using FinanceHogar.Application.DTOs.Alertas;
using FinanceHogar.Domain.Enums;

namespace FinanceHogar.Application.Interfaces;

public interface IAlertasService
{
    Task<IReadOnlyList<AlertaDto>> ObtenerPorHogarAsync(Guid hogarId, EstadoAlerta? estado, CancellationToken ct = default);
    Task<AlertaDto> ObtenerPorIdAsync(Guid id, CancellationToken ct = default);
    Task<int> ContarNoLeidasAsync(Guid hogarId, CancellationToken ct = default);
    Task<AlertaDto> MarcarComoLeidaAsync(Guid id, CancellationToken ct = default);
    Task<AlertaDto> DescartarAsync(Guid id, CancellationToken ct = default);
    Task EliminarAsync(Guid id, CancellationToken ct = default);
    Task GenerarAlertasTemporadaAsync(Guid hogarId, CancellationToken ct = default);
}
