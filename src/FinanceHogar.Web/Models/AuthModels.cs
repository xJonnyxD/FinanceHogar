namespace FinanceHogar.Web.Models;

public record LoginRequest(string Email, string Password);
public record RegisterRequest(string NombreCompleto, string Email, string Password, string? Telefono, string? Dui, string NombreHogar);

public record LoginResponse(
    string Token, string RefreshToken, DateTime Expiration,
    Guid UsuarioId, Guid HogarId, string NombreCompleto, string Email);

public record HogarDto(Guid Id, string Nombre, string? Descripcion, DateTime CreatedAt);

public record CategoriaDto(Guid Id, Guid? HogarId, string Nombre, string? Icono, string? Color, bool EsIngreso, bool EsGlobal);

public record UserSession(Guid UsuarioId, Guid HogarId, string NombreCompleto, string Email, string Token);
