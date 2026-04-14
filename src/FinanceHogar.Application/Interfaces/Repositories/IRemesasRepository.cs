using FinanceHogar.Domain.Entities;

namespace FinanceHogar.Application.Interfaces.Repositories;

public interface IRemesasRepository
{
    Task<Remesa?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Remesa>> ObtenerPorHogarAsync(Guid hogarId, int? anio, int? mes, CancellationToken ct = default);
    Task<decimal> ObtenerTotalAnualAsync(Guid hogarId, int anio, CancellationToken ct = default);
    Task<Remesa> AgregarAsync(Remesa remesa, CancellationToken ct = default);
    Task<Remesa> ActualizarAsync(Remesa remesa, CancellationToken ct = default);
    Task EliminarAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExisteAsync(Guid id, CancellationToken ct = default);
}
