using FinanceHogar.Application.DTOs.ServiciosBasicos;
using FinanceHogar.Application.Interfaces;
using FinanceHogar.Application.Interfaces.Repositories;
using FinanceHogar.Domain.Entities;
using FinanceHogar.Domain.Enums;

namespace FinanceHogar.Application.Services;

public class ServiciosBasicosService : IServiciosBasicosService
{
    private readonly IServiciosBasicosRepository _servicios;
    private readonly IGastosRepository           _gastos;

    public ServiciosBasicosService(
        IServiciosBasicosRepository servicios,
        IGastosRepository gastos)
    {
        _servicios = servicios;
        _gastos    = gastos;
    }

    public async Task<IReadOnlyList<ServicioBasicoDto>> ObtenerPorHogarAsync(Guid hogarId, CancellationToken ct)
    {
        var lista = await _servicios.ObtenerPorHogarAsync(hogarId, ct);
        return lista.Select(Mapear).ToList();
    }

    public async Task<ServicioBasicoDto> ObtenerPorIdAsync(Guid id, CancellationToken ct)
    {
        var s = await _servicios.ObtenerPorIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Servicio básico {id} no encontrado.");
        return Mapear(s);
    }

    public async Task<ServicioBasicoDto> CrearAsync(
        CreateServicioBasicoRequest req, Guid usuarioId, CancellationToken ct)
    {
        var servicio = new ServicioBasico
        {
            HogarId                      = req.HogarId,
            UsuarioId                    = usuarioId,
            TipoServicio                 = (TipoServicio)req.TipoServicio,
            NombreProveedor              = req.NombreProveedor,
            MontoPromedio                = req.MontoPromedio,
            MontoUltimoPago              = req.MontoPromedio,
            FechaVencimiento             = req.FechaVencimiento,
            DiasAnticipacionNotificacion = req.DiasAnticipacionNotificacion
        };
        var creado = await _servicios.AgregarAsync(servicio, ct);
        return Mapear(creado);
    }

    public async Task<ServicioBasicoDto> ActualizarAsync(
        Guid id, UpdateServicioBasicoRequest req, CancellationToken ct)
    {
        var s = await _servicios.ObtenerPorIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Servicio básico {id} no encontrado.");

        s.NombreProveedor              = req.NombreProveedor;
        s.MontoPromedio                = req.MontoPromedio;
        s.FechaVencimiento             = req.FechaVencimiento;
        s.DiasAnticipacionNotificacion = req.DiasAnticipacionNotificacion;
        var actualizado = await _servicios.ActualizarAsync(s, ct);
        return Mapear(actualizado);
    }

    public async Task EliminarAsync(Guid id, CancellationToken ct)
    {
        if (!await _servicios.ExisteAsync(id, ct))
            throw new KeyNotFoundException($"Servicio básico {id} no encontrado.");
        await _servicios.EliminarAsync(id, ct);
    }

    public async Task<IReadOnlyList<ServicioBasicoDto>> ObtenerVencimientosProximosAsync(
        Guid hogarId, int dias, CancellationToken ct)
    {
        var lista = await _servicios.ObtenerProximosVencimientosAsync(hogarId, dias, ct);
        return lista.Select(Mapear).ToList();
    }

    public async Task<ServicioBasicoDto> PagarAsync(Guid id, Guid usuarioId, CancellationToken ct)
    {
        var s = await _servicios.ObtenerPorIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Servicio básico {id} no encontrado.");

        // Crear gasto automático
        var gasto = new Gasto
        {
            UsuarioId        = usuarioId,
            HogarId          = s.HogarId,
            Monto            = s.MontoUltimoPago,
            Moneda           = TipoMoneda.USD,
            MontoEnUSD       = s.MontoUltimoPago,
            Tipo             = TipoGasto.ServicioBasico,
            Descripcion      = $"Pago {s.NombreProveedor}",
            FechaGasto       = DateOnly.FromDateTime(DateTime.UtcNow),
            ServicioBasicoId = s.Id
        };
        await _gastos.AgregarAsync(gasto, ct);

        s.FechaUltimoPago = DateOnly.FromDateTime(DateTime.UtcNow);
        s.EstaVencido     = false;
        // Avanzar vencimiento al siguiente mes
        s.FechaVencimiento = s.FechaVencimiento.AddMonths(1);
        var actualizado = await _servicios.ActualizarAsync(s, ct);
        return Mapear(actualizado);
    }

    private static ServicioBasicoDto Mapear(ServicioBasico s) => new()
    {
        Id                           = s.Id,
        HogarId                      = s.HogarId,
        TipoServicio                 = s.TipoServicio.ToString(),
        NombreProveedor              = s.NombreProveedor,
        MontoPromedio                = s.MontoPromedio ?? s.MontoUltimoPago,
        FechaVencimiento             = s.FechaVencimiento,
        DiasAnticipacionNotificacion = s.DiasAnticipacionNotificacion,
        EstaPagado                   = !s.EstaVencido && s.FechaUltimoPago.HasValue,
        CreatedAt                    = s.CreatedAt
    };
}
