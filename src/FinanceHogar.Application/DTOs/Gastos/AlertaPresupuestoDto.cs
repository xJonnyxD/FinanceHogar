namespace FinanceHogar.Application.DTOs.Gastos;

public class AlertaPresupuestoDto
{
    public string Tipo { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public decimal PorcentajeUso { get; set; }
}
