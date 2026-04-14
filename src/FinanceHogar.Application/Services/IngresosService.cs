using FinanceHogar.Application.DTOs.Ingresos;
using FinanceHogar.Application.Interfaces;
using FinanceHogar.Domain.Entities;
using FinanceHogar.Domain.Enums;
using FinanceHogar.Application.Interfaces.Repositories;

namespace FinanceHogar.Application.Services;

public class IngresosService : IIngresosService
{
    private readonly IIngresosRepository _ingresos;
    private readonly IHogaresRepository _hogares;

    public IngresosService(IIngresosRepository ingresos, IHogaresRepository hogares)
    {
        _ingresos = ingresos;
        _hogares = hogares;
    }

    public async Task<IReadOnlyList<IngresoDto>> ObtenerPorHogarAsync(
        Guid hogarId, DateOnly? desde, DateOnly? hasta, Guid? categoriaId, CancellationToken ct)
    {
        var lista = await _ingresos.ObtenerPorHogarAsync(hogarId, desde, hasta, categoriaId, ct);
        return lista.Select(MapearDto).ToList();
    }

    public async Task<IngresoDto> ObtenerPorIdAsync(Guid id, CancellationToken ct)
    {
        var ingreso = await _ingresos.ObtenerPorIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Ingreso {id} no encontrado.");
        return MapearDto(ingreso);
    }

    public async Task<IngresoDto> RegistrarAsync(CreateIngresoRequest req, Guid usuarioId, CancellationToken ct)
    {
        if (!await _hogares.ExisteAsync(req.HogarId, ct))
            throw new KeyNotFoundException($"Hogar {req.HogarId} no encontrado.");

        var ingreso = new Ingreso
        {
            UsuarioId    = usuarioId,
            HogarId      = req.HogarId,
            CategoriaId  = req.CategoriaId,
            Monto        = req.Monto,
            Moneda       = req.Moneda,
            MontoEnUSD   = req.Moneda == TipoMoneda.USD ? req.Monto : req.MontoEnUSD,
            Tipo         = req.Tipo,
            Descripcion  = req.Descripcion,
            FechaIngreso = req.FechaIngreso,
            EsRecurrente = req.EsRecurrente,
            Frecuencia   = req.Frecuencia,
            RemesaId     = req.RemesaId
        };

        var creado = await _ingresos.AgregarAsync(ingreso, ct);
        return MapearDto(creado);
    }

    public async Task<IngresoDto> ActualizarAsync(Guid id, UpdateIngresoRequest req, Guid usuarioId, CancellationToken ct)
    {
        var ingreso = await _ingresos.ObtenerPorIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Ingreso {id} no encontrado.");

        if (ingreso.UsuarioId != usuarioId)
            throw new UnauthorizedAccessException("No tiene permiso para modificar este ingreso.");

        ingreso.CategoriaId  = req.CategoriaId;
        ingreso.Monto        = req.Monto;
        ingreso.Moneda       = req.Moneda;
        ingreso.MontoEnUSD   = req.Moneda == TipoMoneda.USD ? req.Monto : req.MontoEnUSD;
        ingreso.Tipo         = req.Tipo;
        ingreso.Descripcion  = req.Descripcion;
        ingreso.FechaIngreso = req.FechaIngreso;
        ingreso.EsRecurrente = req.EsRecurrente;
        ingreso.Frecuencia   = req.Frecuencia;

        var actualizado = await _ingresos.ActualizarAsync(ingreso, ct);
        return MapearDto(actualizado);
    }

    public async Task EliminarAsync(Guid id, Guid usuarioId, CancellationToken ct)
    {
        var ingreso = await _ingresos.ObtenerPorIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Ingreso {id} no encontrado.");

        if (ingreso.UsuarioId != usuarioId)
            throw new UnauthorizedAccessException("No tiene permiso para eliminar este ingreso.");

        await _ingresos.EliminarAsync(id, ct);
    }

    public async Task<ResumenMensualIngresosDto> ObtenerResumenMensualAsync(
        Guid hogarId, int anio, int mes, CancellationToken ct)
    {
        var total = await _ingresos.ObtenerTotalMensualAsync(hogarId, anio, mes, ct);
        var (anioAnterior, mesAnterior) = mes == 1 ? (anio - 1, 12) : (anio, mes - 1);
        var totalAnterior = await _ingresos.ObtenerTotalMensualAsync(hogarId, anioAnterior, mesAnterior, ct);

        var variacion = totalAnterior > 0
            ? Math.Round((total - totalAnterior) / totalAnterior * 100, 2)
            : 0;

        var porTipoRaw = await _ingresos.ObtenerTotalesPorTipoAsync(hogarId, anio, mes, ct);

        return new ResumenMensualIngresosDto
        {
            HogarId = hogarId,
            Anio = anio,
            Mes = mes,
            TotalIngresos = total,
            VariacionVsMesAnteriorPct = variacion,
            PorTipo = porTipoRaw.ToDictionary(
                kv => ((TipoIngreso)kv.Key).ToString(),
                kv => kv.Value)
        };
    }

    public async Task<IReadOnlyList<IngresoDto>> ObtenerRecurrentesAsync(Guid hogarId, CancellationToken ct)
    {
        var lista = await _ingresos.ObtenerRecurrentesPorHogarAsync(hogarId, ct);
        return lista.Select(MapearDto).ToList();
    }

    private static IngresoDto MapearDto(Ingreso i) => new()
    {
        Id             = i.Id,
        UsuarioId      = i.UsuarioId,
        NombreUsuario  = i.Usuario?.NombreCompleto ?? string.Empty,
        HogarId        = i.HogarId,
        CategoriaId    = i.CategoriaId,
        NombreCategoria = i.Categoria?.Nombre ?? string.Empty,
        Monto          = i.Monto,
        Moneda         = i.Moneda.ToString(),
        MontoEnUSD     = i.MontoEnUSD,
        Tipo           = i.Tipo.ToString(),
        Descripcion    = i.Descripcion,
        FechaIngreso   = i.FechaIngreso,
        EsRecurrente   = i.EsRecurrente,
        Frecuencia     = i.Frecuencia?.ToString(),
        RemesaId       = i.RemesaId,
        CreatedAt      = i.CreatedAt
    };
}
