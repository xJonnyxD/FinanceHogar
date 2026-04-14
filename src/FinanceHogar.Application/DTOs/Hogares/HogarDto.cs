namespace FinanceHogar.Application.DTOs.Hogares;

public class HogarDto
{
    public Guid    Id                     { get; set; }
    public string  Nombre                 { get; set; } = string.Empty;
    public string  Pais                   { get; set; } = string.Empty;
    public string? Departamento           { get; set; }
    public string? Municipio              { get; set; }
    public string  MonedaPrincipal        { get; set; } = string.Empty;
    public decimal PresupuestoMensualTotal { get; set; }
    public DateTime CreatedAt             { get; set; }
}

public class CreateHogarRequest
{
    public string  Nombre                 { get; set; } = string.Empty;
    public string? Departamento           { get; set; }
    public string? Municipio              { get; set; }
    public string  MonedaPrincipal        { get; set; } = "USD";
    public decimal PresupuestoMensualTotal { get; set; }
}

public class UpdateHogarRequest
{
    public string  Nombre                 { get; set; } = string.Empty;
    public string? Departamento           { get; set; }
    public string? Municipio              { get; set; }
    public decimal PresupuestoMensualTotal { get; set; }
}

public class InvitarMiembroRequest
{
    public string Email           { get; set; } = string.Empty;
    public bool   EsAdministrador { get; set; } = false;
}
