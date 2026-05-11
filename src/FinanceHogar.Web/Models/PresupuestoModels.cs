namespace FinanceHogar.Web.Models;

public record PresupuestoDto(Guid Id, Guid HogarId, Guid CategoriaId, string NombreCategoria,
    int Anio, int Mes, decimal MontoLimite, decimal MontoGastado, decimal PorcentajeUso, DateTime CreatedAt);

public class CreatePresupuestoRequest
{
    public Guid HogarId { get; set; }
    public Guid CategoriaId { get; set; }
    public int Anio { get; set; } = DateTime.Today.Year;
    public int Mes { get; set; } = DateTime.Today.Month;
    public decimal MontoLimite { get; set; }
}
