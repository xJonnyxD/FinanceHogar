using FinanceHogar.Domain.Common;
using FinanceHogar.Domain.Enums;

namespace FinanceHogar.Domain.Entities;

public class ServicioBasico : BaseEntity
{
    public Guid HogarId { get; set; }
    public Hogar Hogar { get; set; } = null!;

    public Guid UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public TipoServicio TipoServicio { get; set; }
    public string NombreProveedor { get; set; } = string.Empty;  // ANDA, DELSUR, etc.
    public decimal MontoUltimoPago { get; set; }
    public decimal? MontoPromedio { get; set; }
    public DateOnly FechaVencimiento { get; set; }
    public DateOnly? FechaUltimoPago { get; set; }
    public bool EstaVencido { get; set; } = false;
    public bool NotificacionActiva { get; set; } = true;
    public int DiasAnticipacionNotificacion { get; set; } = 5;
    public string? NumeroCuenta { get; set; }

    public ICollection<Gasto> Pagos { get; set; } = [];
}
