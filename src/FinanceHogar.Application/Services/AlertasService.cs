using FinanceHogar.Application.DTOs.Alertas;
using FinanceHogar.Application.Interfaces;
using FinanceHogar.Application.Interfaces.Repositories;
using FinanceHogar.Domain.Entities;
using FinanceHogar.Domain.Enums;

namespace FinanceHogar.Application.Services;

public class AlertasService : IAlertasService
{
    private readonly IAlertasRepository _alertas;
    private readonly IHogaresRepository _hogares;

    public AlertasService(IAlertasRepository alertas, IHogaresRepository hogares)
    {
        _alertas = alertas;
        _hogares = hogares;
    }

    public async Task<IReadOnlyList<AlertaDto>> ObtenerPorHogarAsync(
        Guid hogarId, EstadoAlerta? estado, CancellationToken ct)
    {
        var lista = await _alertas.ObtenerPorHogarAsync(hogarId, estado, ct);
        return lista.Select(Mapear).ToList();
    }

    public async Task<AlertaDto> ObtenerPorIdAsync(Guid id, CancellationToken ct)
    {
        var alerta = await _alertas.ObtenerPorIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Alerta {id} no encontrada.");
        return Mapear(alerta);
    }

    public Task<int> ContarNoLeidasAsync(Guid hogarId, CancellationToken ct)
        => _alertas.ContarNoLeidasAsync(hogarId, ct);

    public async Task<AlertaDto> MarcarComoLeidaAsync(Guid id, CancellationToken ct)
    {
        var alerta = await _alertas.ObtenerPorIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Alerta {id} no encontrada.");
        alerta.Estado     = EstadoAlerta.Leida;
        alerta.FechaLeida = DateTime.UtcNow;
        var actualizada = await _alertas.ActualizarAsync(alerta, ct);
        return Mapear(actualizada);
    }

    public async Task<AlertaDto> DescartarAsync(Guid id, CancellationToken ct)
    {
        var alerta = await _alertas.ObtenerPorIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Alerta {id} no encontrada.");
        alerta.Estado = EstadoAlerta.Descartada;
        var actualizada = await _alertas.ActualizarAsync(alerta, ct);
        return Mapear(actualizada);
    }

    public async Task EliminarAsync(Guid id, CancellationToken ct)
    {
        if (await _alertas.ObtenerPorIdAsync(id, ct) is null)
            throw new KeyNotFoundException($"Alerta {id} no encontrada.");
        await _alertas.EliminarAsync(id, ct);
    }

    public async Task GenerarAlertasTemporadaAsync(Guid hogarId, CancellationToken ct)
    {
        if (!await _hogares.ExisteAsync(hogarId, ct))
            throw new KeyNotFoundException($"Hogar {hogarId} no encontrado.");

        var ahora = DateTime.UtcNow;
        var alertasTemporada = new List<(TipoAlerta tipo, string titulo, string mensaje)>();

        if (ahora.Month == 7)   // julio → alerta agosto escolar
        {
            var yaExiste = await _alertas.ExisteAlertaTemporadaEnMesAsync(
                hogarId, TipoAlerta.TemporadaEscolar, ahora.Year, 8, ct);
            if (!yaExiste)
                alertasTemporada.Add((
                    TipoAlerta.TemporadaEscolar,
                    "Temporada Escolar — Agosto se acerca",
                    "Prepara tu presupuesto para útiles, uniformes y matrícula del año escolar."));
        }

        if (ahora.Month == 11)  // noviembre → alerta diciembre navideño
        {
            var yaExiste = await _alertas.ExisteAlertaTemporadaEnMesAsync(
                hogarId, TipoAlerta.TemporadaNavidad, ahora.Year, 12, ct);
            if (!yaExiste)
                alertasTemporada.Add((
                    TipoAlerta.TemporadaNavidad,
                    "Temporada Navideña — Diciembre se acerca",
                    "Planifica gastos navideños y aprovecha el aguinaldo con responsabilidad."));
        }

        foreach (var (tipo, titulo, mensaje) in alertasTemporada)
        {
            await _alertas.AgregarAsync(new Alerta
            {
                HogarId       = hogarId,
                Tipo          = tipo,
                Estado        = EstadoAlerta.Pendiente,
                Titulo        = titulo,
                Mensaje       = mensaje,
                FechaGenerada = ahora
            }, ct);
        }
    }

    private static AlertaDto Mapear(Alerta a) => new()
    {
        Id            = a.Id,
        HogarId       = a.HogarId,
        Tipo          = a.Tipo.ToString(),
        Estado        = a.Estado.ToString(),
        Titulo        = a.Titulo,
        Mensaje       = a.Mensaje,
        PorcentajeUso = a.PorcentajeUso,
        FechaGenerada = a.FechaGenerada,
        FechaLeida    = a.FechaLeida,
        CreatedAt     = a.CreatedAt
    };
}
