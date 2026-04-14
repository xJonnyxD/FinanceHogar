namespace FinanceHogar.Application.DTOs.Alertas;

public class AlertaDto
{
    public Guid      Id            { get; set; }
    public Guid      HogarId       { get; set; }
    public string    Tipo          { get; set; } = string.Empty;
    public string    Estado        { get; set; } = string.Empty;
    public string    Titulo        { get; set; } = string.Empty;
    public string    Mensaje       { get; set; } = string.Empty;
    public decimal?  PorcentajeUso { get; set; }
    public DateTime  FechaGenerada { get; set; }
    public DateTime? FechaLeida    { get; set; }
    public DateTime  CreatedAt     { get; set; }
}
