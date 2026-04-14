using FinanceHogar.Domain.Common;

namespace FinanceHogar.Domain.Entities;

public class TandaParticipante : BaseEntity
{
    public Guid TandaId { get; set; }
    public Tanda Tanda { get; set; } = null!;

    public Guid UsuarioId { get; set; }
    public Usuario Usuario { get; set; } = null!;

    public int NumeroTurno { get; set; }       // en qué ronda recibe el dinero
    public bool HaRecibido { get; set; } = false;
    public DateTime? FechaRecibio { get; set; }
    public int CuotasPagadas { get; set; } = 0;
}
