namespace FinanceHogar.Application.DTOs.Tandas;

public class TandaDto
{
    public Guid    Id                  { get; set; }
    public Guid    HogarId             { get; set; }
    public string  Nombre              { get; set; } = string.Empty;
    public decimal CuotaMensual        { get; set; }
    public int     TotalParticipantes  { get; set; }
    public int     TurnoActual         { get; set; }
    public string  Estado              { get; set; } = string.Empty;
    public DateOnly FechaInicio        { get; set; }
    public List<ParticipanteDto> Participantes { get; set; } = [];
    public DateTime CreatedAt          { get; set; }
}

public class ParticipanteDto
{
    public Guid   UsuarioId      { get; set; }
    public string NombreUsuario  { get; set; } = string.Empty;
    public int    NumeroTurno    { get; set; }
    public bool   HaRecibido     { get; set; }
    public bool   CuotaPagadaMes { get; set; }
}

public class CreateTandaRequest
{
    public Guid     HogarId            { get; set; }
    public string   Nombre             { get; set; } = string.Empty;
    public decimal  CuotaMensual       { get; set; }
    public int      TotalParticipantes { get; set; }
    public DateOnly FechaInicio        { get; set; }
}

public class UpdateTandaRequest
{
    public string  Nombre       { get; set; } = string.Empty;
    public decimal CuotaMensual { get; set; }
}

public class AgregarParticipanteRequest
{
    public Guid UsuarioId   { get; set; }
    public int  NumeroTurno { get; set; }
}
