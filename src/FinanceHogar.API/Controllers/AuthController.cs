using FinanceHogar.Application.DTOs.Auth;
using FinanceHogar.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHogar.API.Controllers;

public class AuthController : BaseController
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth) => _auth = auth;

    /// <summary>Registrar nuevo usuario y crear hogar inicial.</summary>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequest req, CancellationToken ct)
    {
        var response = await _auth.RegisterAsync(req, ct);
        return Ok(response);
    }

    /// <summary>Iniciar sesión y obtener JWT.</summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginRequest req, CancellationToken ct)
    {
        var response = await _auth.LoginAsync(req, ct);
        return Ok(response);
    }

    /// <summary>Renovar JWT usando refresh token.</summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh(RefreshTokenRequest req, CancellationToken ct)
    {
        var response = await _auth.RefreshTokenAsync(req.RefreshToken, ct);
        return Ok(response);
    }

    /// <summary>Cerrar sesión (invalida refresh token).</summary>
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        await _auth.LogoutAsync(UsuarioActualId, ct);
        return NoContent();
    }

    /// <summary>Cambiar contraseña.</summary>
    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword(
        [FromBody] CambiarPasswordRequest req, CancellationToken ct)
    {
        await _auth.CambiarPasswordAsync(UsuarioActualId, req.PasswordActual, req.PasswordNueva, ct);
        return NoContent();
    }
}
