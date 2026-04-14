namespace FinanceHogar.Application.DTOs.Reportes;

public class BalanceMensualDto
{
    public Guid HogarId { get; set; }
    public int Anio { get; set; }
    public int Mes { get; set; }
    public decimal TotalIngresos { get; set; }
    public decimal TotalGastos { get; set; }
    public decimal Balance { get; set; }
    public decimal VariacionIngresosVsMesAnteriorPct { get; set; }
    public decimal VariacionGastosVsMesAnteriorPct { get; set; }
    public List<GastoPorCategoriaDto> GastosPorCategoria { get; set; } = [];
}

public class GastoPorCategoriaDto
{
    public Guid CategoriaId { get; set; }
    public string NombreCategoria { get; set; } = string.Empty;
    public string? Color { get; set; }
    public decimal Total { get; set; }
    public decimal PorcentajeDelTotal { get; set; }
}
