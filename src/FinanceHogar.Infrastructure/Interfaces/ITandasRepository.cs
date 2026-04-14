using FinanceHogar.Domain.Entities;

namespace FinanceHogar.Infrastructure.Interfaces;

public interface ITandasRepository
{
    Task<Tanda?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Tanda>> ObtenerPorHogarAsync(Guid hogarId, CancellationToken ct = default);
    Task<TandaParticipante?> ObtenerParticipanteAsync(Guid tandaId, Guid usuarioId, CancellationToken ct = default);
    Task<Tanda> AgregarAsync(Tanda tanda, CancellationToken ct = default);
    Task<Tanda> ActualizarAsync(Tanda tanda, CancellationToken ct = default);
    Task EliminarAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExisteAsync(Guid id, CancellationToken ct = default);
}
