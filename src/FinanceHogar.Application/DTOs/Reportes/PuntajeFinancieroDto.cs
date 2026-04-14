namespace FinanceHogar.Application.DTOs.Reportes;

public class PuntajeFinancieroDto
{
    public Guid HogarId { get; set; }
    public decimal Puntaje { get; set; }
    public string Nivel { get; set; } = string.Empty;  // Critico, Regular, Bueno, Excelente
    public decimal TasaAhorro { get; set; }
    public bool ServiciosPagadosPuntual { get; set; }
    public List<string> Recomendaciones { get; set; } = [];
}
