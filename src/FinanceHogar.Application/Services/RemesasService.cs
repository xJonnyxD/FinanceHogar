using FinanceHogar.Application.DTOs.Remesas;
using FinanceHogar.Application.Interfaces;
using FinanceHogar.Application.Interfaces.Repositories;
using FinanceHogar.Domain.Entities;
using FinanceHogar.Domain.Enums;

namespace FinanceHogar.Application.Services;

public class RemesasService : IRemesasService
{
    private readonly IRemesasRepository  _remesas;
    private readonly IIngresosRepository _ingresos;

    public RemesasService(IRemesasRepository remesas, IIngresosRepository ingresos)
    {
        _remesas  = remesas;
        _ingresos = ingresos;
    }

    public async Task<IReadOnlyList<RemesaDto>> ObtenerPorHogarAsync(
        Guid hogarId, int? anio, int? mes, CancellationToken ct)
    {
        var lista = await _remesas.ObtenerPorHogarAsync(hogarId, anio, mes, ct);
        return lista.Select(Mapear).ToList();
    }

    public async Task<RemesaDto> ObtenerPorIdAsync(Guid id, CancellationToken ct)
    {
        var r = await _remesas.ObtenerPorIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Remesa {id} no encontrada.");
        return Mapear(r);
    }

    public async Task<RemesaDto> RegistrarAsync(
        CreateRemesaRequest req, Guid receptorId, CancellationToken ct)
    {
        var montoUSD = req.Moneda == "USD" ? req.Monto : req.MontoEnUSD ?? req.Monto;

        var remesa = new Remesa
        {
            HogarId            = req.HogarId,
            ReceptorId         = receptorId,
            Monto              = req.Monto,
            Moneda             = req.Moneda == "BTC" ? TipoMoneda.BTC : TipoMoneda.USD,
            PaisOrigen         = req.PaisOrigen,
            Empresa            = req.Empresa,
            NumeroConfirmacion = req.NumeroConfirmacion,
            FechaRecibida      = req.FechaRecepcion
        };
        var creada = await _remesas.AgregarAsync(remesa, ct);

        // Auto-crear ingreso asociado a la remesa
        await _ingresos.AgregarAsync(new Ingreso
        {
            UsuarioId    = receptorId,
            HogarId      = req.HogarId,
            CategoriaId  = req.CategoriaId,
            Monto        = req.Monto,
            Moneda       = remesa.Moneda,
            MontoEnUSD   = montoUSD,
            Tipo         = TipoIngreso.Remesa,
            Descripcion  = $"Remesa de {req.PaisOrigen} vía {req.Empresa}",
            FechaIngreso = req.FechaRecepcion,
            RemesaId     = creada.Id
        }, ct);

        return Mapear(creada);
    }

    public async Task<RemesaDto> ActualizarAsync(Guid id, UpdateRemesaRequest req, CancellationToken ct)
    {
        var r = await _remesas.ObtenerPorIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Remesa {id} no encontrada.");
        r.Monto              = req.Monto;
        r.Empresa            = req.Empresa;
        r.NumeroConfirmacion = req.NumeroConfirmacion;
        r.FechaRecibida      = req.FechaRecepcion;
        var actualizada = await _remesas.ActualizarAsync(r, ct);
        return Mapear(actualizada);
    }

    public async Task EliminarAsync(Guid id, CancellationToken ct)
    {
        if (!await _remesas.ExisteAsync(id, ct))
            throw new KeyNotFoundException($"Remesa {id} no encontrada.");
        await _remesas.EliminarAsync(id, ct);
    }

    public async Task<EstadisticasRemesasDto> ObtenerEstadisticasAsync(
        Guid hogarId, int anio, CancellationToken ct)
    {
        var lista = await _remesas.ObtenerPorHogarAsync(hogarId, anio, null, ct);
        var total = await _remesas.ObtenerTotalAnualAsync(hogarId, anio, ct);

        return new EstadisticasRemesasDto
        {
            TotalRemesas          = lista.Count,
            MontoTotalAnual       = total,
            PromedioMensual       = lista.Count > 0 ? total / 12 : 0,
            EmpresaMasFrecuente   = lista
                .GroupBy(r => r.Empresa ?? string.Empty)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key ?? string.Empty,
            PaisOrigenMasFrecuente = lista
                .GroupBy(r => r.PaisOrigen ?? string.Empty)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault()?.Key ?? string.Empty
        };
    }

    private static RemesaDto Mapear(Remesa r) => new()
    {
        Id                 = r.Id,
        HogarId            = r.HogarId,
        ReceptorId         = r.ReceptorId,
        NombreReceptor     = r.Receptor?.NombreCompleto ?? string.Empty,
        Monto              = r.Monto,
        Moneda             = r.Moneda.ToString(),
        PaisOrigen         = r.PaisOrigen ?? string.Empty,
        Empresa            = r.Empresa ?? string.Empty,
        NumeroConfirmacion = r.NumeroConfirmacion,
        FechaRecepcion     = r.FechaRecibida,
        CreatedAt          = r.CreatedAt
    };
}
