using FinanceHogar.Domain.Enums;

namespace FinanceHogar.Application.DTOs.Ingresos;

public class UpdateIngresoRequest
{
    public Guid CategoriaId { get; set; }
    public decimal Monto { get; set; }
    public TipoMoneda Moneda { get; set; } = TipoMoneda.USD;
    public decimal? MontoEnUSD { get; set; }
    public TipoIngreso Tipo { get; set; }
    public string? Descripcion { get; set; }
    public DateOnly FechaIngreso { get; set; }
    public bool EsRecurrente { get; set; }
    public TipoFrecuencia? Frecuencia { get; set; }
}
