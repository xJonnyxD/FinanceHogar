using FinanceHogar.Domain.Common;

namespace FinanceHogar.Domain.Entities;

public class PresupuestoMensual : BaseEntity
{
    public Guid HogarId { get; set; }
    public Hogar Hogar { get; set; } = null!;

    public Guid CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!;

    public int Anio { get; set; }
    public int Mes { get; set; }  // 1-12

    public decimal MontoLimite { get; set; }
    public decimal MontoGastado { get; set; } = 0;

    public decimal PorcentajeUso =>
        MontoLimite > 0 ? Math.Round(MontoGastado / MontoLimite * 100, 2) : 0;
}
