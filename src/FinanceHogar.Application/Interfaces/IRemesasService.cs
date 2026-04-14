using FinanceHogar.Application.DTOs.Remesas;

namespace FinanceHogar.Application.Interfaces;

public interface IRemesasService
{
    Task<IReadOnlyList<RemesaDto>> ObtenerPorHogarAsync(Guid hogarId, int? anio, int? mes, CancellationToken ct = default);
    Task<RemesaDto> ObtenerPorIdAsync(Guid id, CancellationToken ct = default);
    Task<RemesaDto> RegistrarAsync(CreateRemesaRequest request, Guid receptorId, CancellationToken ct = default);
    Task<RemesaDto> ActualizarAsync(Guid id, UpdateRemesaRequest request, CancellationToken ct = default);
    Task EliminarAsync(Guid id, CancellationToken ct = default);
    Task<EstadisticasRemesasDto> ObtenerEstadisticasAsync(Guid hogarId, int anio, CancellationToken ct = default);
}
