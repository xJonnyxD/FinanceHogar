using FinanceHogar.Application.DTOs.Auth;

namespace FinanceHogar.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> RegisterAsync(RegisterRequest request, CancellationToken ct = default);
    Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken ct = default);
    Task<LoginResponse> RefreshTokenAsync(string refreshToken, CancellationToken ct = default);
    Task LogoutAsync(Guid usuarioId, CancellationToken ct = default);
    Task CambiarPasswordAsync(Guid usuarioId, string passwordActual, string passwordNueva, CancellationToken ct = default);
}
