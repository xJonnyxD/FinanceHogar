namespace FinanceHogar.Application.DTOs.Ingresos;

public class IngresoDto
{
    public Guid Id { get; set; }
    public Guid UsuarioId { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
    public Guid HogarId { get; set; }
    public Guid CategoriaId { get; set; }
    public string NombreCategoria { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public string Moneda { get; set; } = string.Empty;
    public decimal? MontoEnUSD { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public DateOnly FechaIngreso { get; set; }
    public bool EsRecurrente { get; set; }
    public string? Frecuencia { get; set; }
    public Guid? RemesaId { get; set; }
    public DateTime CreatedAt { get; set; }
}
