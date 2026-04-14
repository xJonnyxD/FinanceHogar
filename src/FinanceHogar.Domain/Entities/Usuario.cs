using FinanceHogar.Domain.Common;

namespace FinanceHogar.Domain.Entities;

public class Usuario : BaseEntity
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? DUI { get; set; }  // Documento Único de Identidad — El Salvador
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }
    public bool EstaActivo { get; set; } = true;

    public ICollection<HogarUsuario> HogarUsuarios { get; set; } = [];
    public ICollection<Ingreso> Ingresos { get; set; } = [];
    public ICollection<Gasto> Gastos { get; set; } = [];
    public ICollection<ServicioBasico> ServiciosBasicos { get; set; } = [];
    public ICollection<Remesa> RemesasRecibidas { get; set; } = [];
}
