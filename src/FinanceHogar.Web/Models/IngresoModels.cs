namespace FinanceHogar.Web.Models;

public record IngresoDto(Guid Id, Guid HogarId, Guid CategoriaId, string NombreCategoria,
    decimal Monto, string Moneda, string Tipo, string? Descripcion,
    DateOnly FechaIngreso, bool EsRecurrente, string? Frecuencia, DateTime CreatedAt);

public class CreateIngresoRequest
{
    public Guid HogarId { get; set; }
    public Guid CategoriaId { get; set; }
    public decimal Monto { get; set; }
    public int Moneda { get; set; } = 1;
    public int Tipo { get; set; } = 1;
    public string? Descripcion { get; set; }
    public DateOnly FechaIngreso { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public bool EsRecurrente { get; set; }
    public int? Frecuencia { get; set; }
}
