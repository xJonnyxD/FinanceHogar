using FinanceHogar.Domain.Common;
using FinanceHogar.Domain.Enums;

namespace FinanceHogar.Domain.Entities;

public class Hogar : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public string Pais { get; set; } = "El Salvador";
    public string? Departamento { get; set; }
    public string? Municipio { get; set; }
    public TipoMoneda MonedaPrincipal { get; set; } = TipoMoneda.USD;
    public decimal PresupuestoMensualTotal { get; set; }

    public ICollection<HogarUsuario> HogarUsuarios { get; set; } = [];
    public ICollection<Categoria> Categorias { get; set; } = [];
    public ICollection<PresupuestoMensual> Presupuestos { get; set; } = [];
    public ICollection<Alerta> Alertas { get; set; } = [];
    public ICollection<Tanda> Tandas { get; set; } = [];
    public ICollection<Remesa> Remesas { get; set; } = [];
}
