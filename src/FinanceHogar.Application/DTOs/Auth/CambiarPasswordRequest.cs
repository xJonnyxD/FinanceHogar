namespace FinanceHogar.Application.DTOs.Auth;

public class CambiarPasswordRequest
{
    public string PasswordActual { get; set; } = string.Empty;
    public string PasswordNueva  { get; set; } = string.Empty;
}
