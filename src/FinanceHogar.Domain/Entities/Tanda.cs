using FinanceHogar.Domain.Common;
using FinanceHogar.Domain.Enums;

namespace FinanceHogar.Domain.Entities;

// Sistema de ahorro rotativo — muy común en El Salvador y Centroamérica
public class Tanda : BaseEntity
{
    public Guid HogarId { get; set; }
    public Hogar Hogar { get; set; } = null!;

    public Guid OrganizadorId { get; set; }
    public Usuario Organizador { get; set; } = null!;

    public string Nombre { get; set; } = string.Empty;
    public decimal CuotaMensual { get; set; }
    public int TotalParticipantes { get; set; }
    public TipoFrecuencia Frecuencia { get; set; } = TipoFrecuencia.Mensual;
    public DateOnly FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }
    public EstadoTanda Estado { get; set; } = EstadoTanda.Activa;
    public int TurnoActual { get; set; } = 1;

    public ICollection<TandaParticipante> Participantes { get; set; } = [];
}
