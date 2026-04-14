using FinanceHogar.Domain.Common;

namespace FinanceHogar.Domain.Entities;

public class Rol : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;  // "Admin", "Miembro"
    public string? Descripcion { get; set; }

    public ICollection<HogarUsuario> HogarUsuarios { get; set; } = [];
}
