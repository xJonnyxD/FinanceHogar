using FinanceHogar.Domain.Common;

namespace FinanceHogar.Domain.Entities;

public class HogarUsuario : BaseEntity
{
    public Guid HogarId { get; set; }
    public Hogar Hogar { get; set; } = null!;

    public Guid UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public Guid RolId { get; set; }
    public Rol Rol { get; set; } = null!;

    public bool EsAdministrador { get; set; } = false;
    public DateTime FechaIngreso { get; set; } = DateTime.UtcNow;
}
