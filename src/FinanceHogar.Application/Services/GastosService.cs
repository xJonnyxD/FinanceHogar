using FinanceHogar.Application.DTOs.Gastos;
using FinanceHogar.Application.Interfaces;
using FinanceHogar.Domain.BusinessRules;
using FinanceHogar.Domain.Entities;
using FinanceHogar.Domain.Enums;
using FinanceHogar.Application.Interfaces.Repositories;

namespace FinanceHogar.Application.Services;

public class GastosService : IGastosService
{
    private readonly IGastosRepository _gastos;
    private readonly IHogaresRepository _hogares;
    private readonly IPresupuestosRepository _presupuestos;
    private readonly IAlertasRepository _alertas;

    public GastosService(
        IGastosRepository gastos,
        IHogaresRepository hogares,
        IPresupuestosRepository presupuestos,
        IAlertasRepository alertas)
    {
        _gastos = gastos;
        _hogares = hogares;
        _presupuestos = presupuestos;
        _alertas = alertas;
    }

    public async Task<IReadOnlyList<GastoDto>> ObtenerPorHogarAsync(
        Guid hogarId, DateOnly? desde, DateOnly? hasta, Guid? categoriaId, CancellationToken ct)
    {
        var lista = await _gastos.ObtenerPorHogarAsync(hogarId, desde, hasta, categoriaId, ct);
        return lista.Select(g => MapearDto(g, null)).ToList();
    }

    public async Task<GastoDto> ObtenerPorIdAsync(Guid id, CancellationToken ct)
    {
        var gasto = await _gastos.ObtenerPorIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Gasto {id} no encontrado.");
        return MapearDto(gasto, null);
    }

    public async Task<GastoDto> RegistrarAsync(CreateGastoRequest req, Guid usuarioId, CancellationToken ct)
    {
        if (!await _hogares.ExisteAsync(req.HogarId, ct))
            throw new KeyNotFoundException($"Hogar {req.HogarId} no encontrado.");

        var montoUSD = req.Moneda == TipoMoneda.USD ? req.Monto : req.MontoEnUSD ?? req.Monto;

        var gasto = new Gasto
        {
            UsuarioId       = usuarioId,
            HogarId         = req.HogarId,
            CategoriaId     = req.CategoriaId,
            Monto           = req.Monto,
            Moneda          = req.Moneda,
            MontoEnUSD      = montoUSD,
            Tipo            = req.Tipo,
            Descripcion     = req.Descripcion,
            FechaGasto      = req.FechaGasto,
            EsRecurrente    = req.EsRecurrente,
            Frecuencia      = req.Frecuencia,
            Comprobante     = req.Comprobante,
            ServicioBasicoId = req.ServicioBasicoId
        };

        var creado = await _gastos.AgregarAsync(gasto, ct);

        // Verificar presupuesto y generar alerta si cruza un umbral
        AlertaPresupuestoDto? alertaDto = null;
        var hoy = req.FechaGasto;
        var presupuesto = await _presupuestos.ObtenerPorHogarCategoriaYMesAsync(
            req.HogarId, req.CategoriaId, hoy.Year, hoy.Month, ct);

        if (presupuesto is not null)
        {
            var pctAnterior = presupuesto.PorcentajeUso;
            presupuesto.MontoGastado += montoUSD;
            await _presupuestos.ActualizarAsync(presupuesto, ct);
            var pctNuevo = presupuesto.PorcentajeUso;

            var tipoAlerta = AlertaRules.EvaluarPresupuesto(pctNuevo, pctAnterior);
            if (tipoAlerta.HasValue)
            {
                var mensaje = tipoAlerta.Value switch
                {
                    TipoAlerta.PresupuestoAlCincuentaPorciento =>
                        $"Has gastado el 50% del presupuesto de {req.CategoriaId}. Llevas ${presupuesto.MontoGastado:F2} de ${presupuesto.MontoLimite:F2}.",
                    TipoAlerta.PresupuestoAlOchentaPorciento =>
                        $"Advertencia: 80% del presupuesto consumido. Quedan ${presupuesto.MontoLimite - presupuesto.MontoGastado:F2}.",
                    TipoAlerta.PresupuestoSuperado =>
                        $"Presupuesto superado. Has gastado ${presupuesto.MontoGastado:F2} de un límite de ${presupuesto.MontoLimite:F2}.",
                    _ => string.Empty
                };

                var alerta = new Alerta
                {
                    HogarId       = req.HogarId,
                    UsuarioId     = usuarioId,
                    CategoriaId   = req.CategoriaId,
                    Tipo          = tipoAlerta.Value,
                    Estado        = EstadoAlerta.Pendiente,
                    Titulo        = tipoAlerta.Value.ToString(),
                    Mensaje       = mensaje,
                    PorcentajeUso = pctNuevo,
                    FechaGenerada = DateTime.UtcNow
                };
                await _alertas.AgregarAsync(alerta, ct);

                alertaDto = new AlertaPresupuestoDto
                {
                    Tipo = tipoAlerta.Value.ToString(),
                    Mensaje = mensaje,
                    PorcentajeUso = pctNuevo
                };
            }
        }

        return MapearDto(creado, alertaDto);
    }

    public async Task<GastoDto> ActualizarAsync(Guid id, UpdateGastoRequest req, Guid usuarioId, CancellationToken ct)
    {
        var gasto = await _gastos.ObtenerPorIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Gasto {id} no encontrado.");

        if (gasto.UsuarioId != usuarioId)
            throw new UnauthorizedAccessException("No tiene permiso para modificar este gasto.");

        gasto.CategoriaId  = req.CategoriaId;
        gasto.Monto        = req.Monto;
        gasto.Moneda       = req.Moneda;
        gasto.MontoEnUSD   = req.Moneda == TipoMoneda.USD ? req.Monto : req.MontoEnUSD;
        gasto.Tipo         = req.Tipo;
        gasto.Descripcion  = req.Descripcion;
        gasto.FechaGasto   = req.FechaGasto;
        gasto.EsRecurrente = req.EsRecurrente;
        gasto.Frecuencia   = req.Frecuencia;
        gasto.Comprobante  = req.Comprobante;

        var actualizado = await _gastos.ActualizarAsync(gasto, ct);
        return MapearDto(actualizado, null);
    }

    public async Task EliminarAsync(Guid id, Guid usuarioId, CancellationToken ct)
    {
        var gasto = await _gastos.ObtenerPorIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Gasto {id} no encontrado.");

        if (gasto.UsuarioId != usuarioId)
            throw new UnauthorizedAccessException("No tiene permiso para eliminar este gasto.");

        await _gastos.EliminarAsync(id, ct);
    }

    public async Task<IReadOnlyList<GastoDto>> ObtenerRecurrentesAsync(Guid hogarId, CancellationToken ct)
    {
        var lista = await _gastos.ObtenerRecurrentesPorHogarAsync(hogarId, ct);
        return lista.Select(g => MapearDto(g, null)).ToList();
    }

    private static GastoDto MapearDto(Gasto g, AlertaPresupuestoDto? alerta) => new()
    {
        Id               = g.Id,
        UsuarioId        = g.UsuarioId,
        NombreUsuario    = g.Usuario?.NombreCompleto ?? string.Empty,
        HogarId          = g.HogarId,
        CategoriaId      = g.CategoriaId,
        NombreCategoria  = g.Categoria?.Nombre ?? string.Empty,
        Monto            = g.Monto,
        Moneda           = g.Moneda.ToString(),
        MontoEnUSD       = g.MontoEnUSD,
        Tipo             = g.Tipo.ToString(),
        Descripcion      = g.Descripcion,
        FechaGasto       = g.FechaGasto,
        EsRecurrente     = g.EsRecurrente,
        Frecuencia       = g.Frecuencia?.ToString(),
        Comprobante      = g.Comprobante,
        ServicioBasicoId = g.ServicioBasicoId,
        CreatedAt        = g.CreatedAt,
        AlertaGenerada   = alerta
    };
}
