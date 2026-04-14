using FinanceHogar.Domain.Common;
using FinanceHogar.Domain.Enums;

namespace FinanceHogar.Domain.Entities;

public class Alerta : BaseEntity
{
    public Guid HogarId { get; set; }
    public Hogar Hogar { get; set; } = null!;

    public Guid? UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public Guid? CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }

    public TipoAlerta Tipo { get; set; }
    public EstadoAlerta Estado { get; set; } = EstadoAlerta.Pendiente;
    public string Titulo { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;
    public decimal? PorcentajeUso { get; set; }
    public DateTime FechaGenerada { get; set; } = DateTime.UtcNow;
    public DateTime? FechaLeida { get; set; }
}
