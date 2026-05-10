using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FinanceHogar.Application.DTOs.Auth;
using FinanceHogar.Application.Interfaces;
using FinanceHogar.Application.Interfaces.Repositories;
using FinanceHogar.Domain.Entities;
using FinanceHogar.Infrastructure.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FinanceHogar.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUsuariosRepository _usuarios;
    private readonly IHogaresRepository  _hogares;
    private readonly IConfiguration      _config;
    private readonly AppDbContext        _db;

    public AuthService(
        IUsuariosRepository usuarios,
        IHogaresRepository hogares,
        IConfiguration config,
        AppDbContext db)
    {
        _usuarios = usuarios;
        _hogares  = hogares;
        _config   = config;
        _db       = db;
    }

    public async Task<LoginResponse> RegisterAsync(RegisterRequest req, CancellationToken ct = default)
    {
        if (await _usuarios.ExisteEmailAsync(req.Email, ct))
            throw new ArgumentException($"El correo '{req.Email}' ya está registrado.");

        var usuario = new Usuario
        {
            NombreCompleto = req.NombreCompleto,
            Email          = req.Email,
            PasswordHash   = BCrypt.Net.BCrypt.HashPassword(req.Password),
            Telefono       = req.Telefono,
            DUI            = req.DUI
        };

        usuario = await _usuarios.AgregarAsync(usuario, ct);

        // Crear hogar inicial y asignar como administrador
        var hogar = new Hogar { Nombre = req.NombreHogar };
        hogar = await _hogares.AgregarAsync(hogar, ct);

        // HogarUsuario — relación pivot (RolId Admin = seed fijo)
        var rolAdminId = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000001");
        var hogarUsuario = new HogarUsuario
        {
            HogarId         = hogar.Id,
            UsuarioId       = usuario.Id,
            RolId           = rolAdminId,
            EsAdministrador = true
        };
        await _db.HogarUsuarios.AddAsync(hogarUsuario, ct);
        await _db.SaveChangesAsync(ct);

        return await GenerarLoginResponseAsync(usuario, hogar.Id, esAdmin: true, ct);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest req, CancellationToken ct = default)
    {
        var usuario = await _usuarios.ObtenerPorEmailAsync(req.Email, ct)
            ?? throw new UnauthorizedAccessException("Credenciales incorrectas.");

        if (!BCrypt.Net.BCrypt.Verify(req.Password, usuario.PasswordHash))
            throw new UnauthorizedAccessException("Credenciales incorrectas.");

        var hogarUsuario = usuario.HogarUsuarios?.FirstOrDefault();
        return await GenerarLoginResponseAsync(
            usuario,
            hogarUsuario?.HogarId ?? Guid.Empty,
            hogarUsuario?.EsAdministrador ?? false,
            ct);
    }

    public async Task<LoginResponse> RefreshTokenAsync(string refreshToken, CancellationToken ct = default)
    {
        var usuario = await _usuarios.ObtenerPorRefreshTokenAsync(refreshToken, ct)
            ?? throw new UnauthorizedAccessException("Refresh token inválido o expirado.");

        var hogarUsuario = usuario.HogarUsuarios?.FirstOrDefault();
        return await GenerarLoginResponseAsync(
            usuario,
            hogarUsuario?.HogarId ?? Guid.Empty,
            hogarUsuario?.EsAdministrador ?? false,
            ct);
    }

    public async Task LogoutAsync(Guid usuarioId, CancellationToken ct = default)
    {
        var usuario = await _usuarios.ObtenerPorIdAsync(usuarioId, ct)
            ?? throw new KeyNotFoundException("Usuario no encontrado.");

        usuario.RefreshToken       = null;
        usuario.RefreshTokenExpiry = null;
        await _usuarios.ActualizarAsync(usuario, ct);
    }

    public async Task CambiarPasswordAsync(Guid usuarioId, string passwordActual, string passwordNueva, CancellationToken ct = default)
    {
        var usuario = await _usuarios.ObtenerPorIdAsync(usuarioId, ct)
            ?? throw new KeyNotFoundException("Usuario no encontrado.");

        if (!BCrypt.Net.BCrypt.Verify(passwordActual, usuario.PasswordHash))
            throw new ArgumentException("La contraseña actual es incorrecta.");

        usuario.PasswordHash = BCrypt.Net.BCrypt.HashPassword(passwordNueva);
        await _usuarios.ActualizarAsync(usuario, ct);
    }

    // ─────────────────────────────────────────────────────────────────────────

    private async Task<LoginResponse> GenerarLoginResponseAsync(
        Usuario usuario, Guid hogarId, bool esAdmin, CancellationToken ct)
    {
        var (token, expiration) = GenerarJwt(usuario, hogarId, esAdmin);
        var refreshToken        = GenerarRefreshToken();

        var expirationDays = int.Parse(_config["JwtSettings:RefreshTokenExpirationDays"] ?? "7");
        usuario.RefreshToken       = refreshToken;
        usuario.RefreshTokenExpiry = DateTime.UtcNow.AddDays(expirationDays);
        await _usuarios.ActualizarAsync(usuario, ct);

        return new LoginResponse
        {
            Token         = token,
            RefreshToken  = refreshToken,
            Expiration    = expiration,
            UsuarioId     = usuario.Id,
            HogarId       = hogarId,
            NombreCompleto = usuario.NombreCompleto,
            Email         = usuario.Email
        };
    }

    private (string token, DateTime expiration) GenerarJwt(Usuario usuario, Guid hogarId, bool esAdmin)
    {
        var jwt     = _config.GetSection("JwtSettings");
        var key     = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["SecretKey"]!));
        var creds   = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var minutes = int.Parse(jwt["ExpirationMinutes"] ?? "60");
        var exp     = DateTime.UtcNow.AddMinutes(minutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,   usuario.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
            new Claim(ClaimTypes.NameIdentifier,     usuario.Id.ToString()),
            new Claim("NombreCompleto",              usuario.NombreCompleto),
            new Claim("HogarId",                    hogarId.ToString()),
            new Claim("EsAdministrador",             esAdmin.ToString().ToLower()),
            new Claim(JwtRegisteredClaimNames.Jti,  Guid.NewGuid().ToString())
        };

        var tokenObj = new JwtSecurityToken(
            issuer:             jwt["Issuer"],
            audience:           jwt["Audience"],
            claims:             claims,
            expires:            exp,
            signingCredentials: creds);

        return (new JwtSecurityTokenHandler().WriteToken(tokenObj), exp);
    }

    private static string GenerarRefreshToken()
    {
        var bytes = new byte[64];
        RandomNumberGenerator.Fill(bytes);
        return Convert.ToBase64String(bytes);
    }
}
