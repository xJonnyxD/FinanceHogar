using FinanceHogar.Domain.Common;
using FinanceHogar.Domain.Enums;

namespace FinanceHogar.Domain.Entities;

public class Gasto : BaseEntity
{
    public Guid UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public Guid HogarId { get; set; }
    public Hogar Hogar { get; set; } = null!;

    public Guid CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!;

    public Guid? ServicioBasicoId { get; set; }
    public ServicioBasico? ServicioBasico { get; set; }

    public decimal Monto { get; set; }
    public TipoMoneda Moneda { get; set; } = TipoMoneda.USD;
    public decimal? MontoEnUSD { get; set; }

    public TipoGasto Tipo { get; set; }
    public string? Descripcion { get; set; }
    public DateOnly FechaGasto { get; set; }
    public bool EsRecurrente { get; set; } = false;
    public TipoFrecuencia? Frecuencia { get; set; }
    public string? Comprobante { get; set; }  // URL o ruta del recibo
}
