namespace FinanceHogar.Application.DTOs.ServiciosBasicos;

public class ServicioBasicoDto
{
    public Guid     Id                           { get; set; }
    public Guid     HogarId                      { get; set; }
    public string   TipoServicio                 { get; set; } = string.Empty;
    public string   NombreProveedor              { get; set; } = string.Empty;
    public decimal  MontoPromedio                { get; set; }
    public DateOnly FechaVencimiento             { get; set; }
    public int      DiasAnticipacionNotificacion { get; set; }
    public bool     EstaPagado                   { get; set; }
    public DateTime CreatedAt                    { get; set; }
}

public class CreateServicioBasicoRequest
{
    public Guid     HogarId                      { get; set; }
    public int      TipoServicio                 { get; set; }
    public string   NombreProveedor              { get; set; } = string.Empty;
    public decimal  MontoPromedio                { get; set; }
    public DateOnly FechaVencimiento             { get; set; }
    public int      DiasAnticipacionNotificacion { get; set; } = 5;
}

public class UpdateServicioBasicoRequest
{
    public string   NombreProveedor              { get; set; } = string.Empty;
    public decimal  MontoPromedio                { get; set; }
    public DateOnly FechaVencimiento             { get; set; }
    public int      DiasAnticipacionNotificacion { get; set; }
}
