namespace FinanceHogar.Application.DTOs.Ingresos;

public class ResumenMensualIngresosDto
{
    public Guid HogarId { get; set; }
    public int Anio { get; set; }
    public int Mes { get; set; }
    public decimal TotalIngresos { get; set; }
    public decimal VariacionVsMesAnteriorPct { get; set; }
    public Dictionary<string, decimal> PorTipo { get; set; } = [];
}
