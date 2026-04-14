using FinanceHogar.Domain.Entities;

namespace FinanceHogar.Infrastructure.Interfaces;

public interface IHogaresRepository
{
    Task<Hogar?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Hogar>> ObtenerPorUsuarioAsync(Guid usuarioId, CancellationToken ct = default);
    Task<Hogar> AgregarAsync(Hogar hogar, CancellationToken ct = default);
    Task<Hogar> ActualizarAsync(Hogar hogar, CancellationToken ct = default);
    Task EliminarAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExisteAsync(Guid id, CancellationToken ct = default);
    Task<bool> UsuarioPerteneceAlHogarAsync(Guid hogarId, Guid usuarioId, CancellationToken ct = default);
    Task<bool> UsuarioEsAdminAsync(Guid hogarId, Guid usuarioId, CancellationToken ct = default);
}
