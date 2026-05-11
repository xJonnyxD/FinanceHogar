namespace FinanceHogar.Web.Models;

public record GastoDto(Guid Id, Guid HogarId, Guid CategoriaId, string NombreCategoria,
    decimal Monto, string Moneda, string Tipo, string? Descripcion,
    DateOnly FechaGasto, bool EsRecurrente, string? Frecuencia, DateTime CreatedAt);

public class CreateGastoRequest
{
    public Guid HogarId { get; set; }
    public Guid CategoriaId { get; set; }
    public decimal Monto { get; set; }
    public int Moneda { get; set; } = 1;
    public int Tipo { get; set; } = 1;
    public string? Descripcion { get; set; }
    public DateOnly FechaGasto { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public bool EsRecurrente { get; set; }
    public int? Frecuencia { get; set; }
}

public class UpdateGastoRequest
{
    public Guid CategoriaId { get; set; }
    public decimal Monto { get; set; }
    public int Moneda { get; set; } = 1;
    public int Tipo { get; set; } = 1;
    public string? Descripcion { get; set; }
    public DateOnly FechaGasto { get; set; }
    public bool EsRecurrente { get; set; }
    public int? Frecuencia { get; set; }
}
