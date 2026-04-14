namespace FinanceHogar.Application.DTOs.Gastos;

public class GastoDto
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
    public DateOnly FechaGasto { get; set; }
    public bool EsRecurrente { get; set; }
    public string? Frecuencia { get; set; }
    public string? Comprobante { get; set; }
    public Guid? ServicioBasicoId { get; set; }
    public DateTime CreatedAt { get; set; }
    public AlertaPresupuestoDto? AlertaGenerada { get; set; }
}
