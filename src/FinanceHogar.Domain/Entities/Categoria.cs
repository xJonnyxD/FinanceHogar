using FinanceHogar.Domain.Common;

namespace FinanceHogar.Domain.Entities;

public class Categoria : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string? Icono { get; set; }
    public string? Color { get; set; }      // hex: #RRGGBB
    public bool EsIngreso { get; set; }     // true=ingreso, false=gasto
    public bool EsGlobal { get; set; }      // global (sistema) vs personalizada del hogar
    public Guid? HogarId { get; set; }
    public Hogar? Hogar { get; set; }

    public ICollection<Ingreso> Ingresos { get; set; } = [];
    public ICollection<Gasto> Gastos { get; set; } = [];
    public ICollection<PresupuestoMensual> Presupuestos { get; set; } = [];
}
