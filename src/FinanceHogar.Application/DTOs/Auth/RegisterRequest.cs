namespace FinanceHogar.Application.DTOs.Auth;

public class RegisterRequest
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? DUI { get; set; }
    public string NombreHogar { get; set; } = string.Empty;  // crea o une al hogar
}
