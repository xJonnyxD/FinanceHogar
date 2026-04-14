using FinanceHogar.Domain.Common;
using FinanceHogar.Domain.Enums;

namespace FinanceHogar.Domain.Entities;

// El Salvador recibe ~$8B anuales en remesas — principal fuente de ingreso para ~30% de familias
public class Remesa : BaseEntity
{
    public Guid HogarId { get; set; }
    public Hogar Hogar { get; set; } = null!;

    public Guid ReceptorId { get; set; }
    public Usuario Receptor { get; set; } = null!;

    public decimal Monto { get; set; }
    public TipoMoneda Moneda { get; set; } = TipoMoneda.USD;
    public string? PaisOrigen { get; set; } = "Estados Unidos";
    public string? NombreRemitente { get; set; }
    public string? Empresa { get; set; }            // Western Union, MoneyGram, Remitly, etc.
    public decimal? ComisionCobrada { get; set; }
    public DateOnly FechaRecibida { get; set; }
    public string? NumeroConfirmacion { get; set; }
    public string? Proposito { get; set; }

    public ICollection<Ingreso> Ingresos { get; set; } = [];
}
