namespace FinanceHogar.Application.DTOs.Usuarios;

public class UsuarioDto
{
    public Guid    Id             { get; set; }
    public string  NombreCompleto { get; set; } = string.Empty;
    public string  Email          { get; set; } = string.Empty;
    public string? Telefono       { get; set; }
    public string? DUI            { get; set; }
    public DateTime CreatedAt     { get; set; }
}

public class UpdateUsuarioRequest
{
    public string  NombreCompleto { get; set; } = string.Empty;
    public string? Telefono       { get; set; }
    public string? DUI            { get; set; }
}
