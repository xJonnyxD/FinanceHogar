using FinanceHogar.Domain.Entities;

namespace FinanceHogar.Infrastructure.Interfaces;

public interface IUsuariosRepository
{
    Task<Usuario?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default);
    Task<Usuario?> ObtenerPorEmailAsync(string email, CancellationToken ct = default);
    Task<Usuario?> ObtenerPorRefreshTokenAsync(string refreshToken, CancellationToken ct = default);
    Task<Usuario> AgregarAsync(Usuario usuario, CancellationToken ct = default);
    Task<Usuario> ActualizarAsync(Usuario usuario, CancellationToken ct = default);
    Task<bool> ExisteEmailAsync(string email, CancellationToken ct = default);
}
