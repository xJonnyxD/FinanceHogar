namespace FinanceHogar.Application.DTOs.Remesas;

public class RemesaDto
{
    public Guid      Id                  { get; set; }
    public Guid      HogarId             { get; set; }
    public Guid      ReceptorId          { get; set; }
    public string    NombreReceptor      { get; set; } = string.Empty;
    public decimal   Monto               { get; set; }
    public string    Moneda              { get; set; } = string.Empty;
    public decimal?  MontoEnUSD          { get; set; }
    public string    PaisOrigen          { get; set; } = string.Empty;
    public string    Empresa             { get; set; } = string.Empty;
    public string?   NumeroConfirmacion  { get; set; }
    public DateOnly  FechaRecepcion      { get; set; }
    public DateTime  CreatedAt           { get; set; }
}

public class CreateRemesaRequest
{
    public Guid     HogarId            { get; set; }
    public Guid     CategoriaId        { get; set; }   // categoría "Remesa" del hogar
    public decimal  Monto              { get; set; }
    public string   Moneda             { get; set; } = "USD";
    public decimal? MontoEnUSD         { get; set; }
    public string   PaisOrigen         { get; set; } = "Estados Unidos";
    public string   Empresa            { get; set; } = string.Empty;
    public string?  NumeroConfirmacion { get; set; }
    public DateOnly FechaRecepcion     { get; set; }
}

public class UpdateRemesaRequest
{
    public decimal  Monto              { get; set; }
    public string   Empresa            { get; set; } = string.Empty;
    public string?  NumeroConfirmacion { get; set; }
    public DateOnly FechaRecepcion     { get; set; }
}

public class EstadisticasRemesasDto
{
    public int     TotalRemesas    { get; set; }
    public decimal MontoTotalAnual { get; set; }
    public decimal PromedioMensual { get; set; }
    public string  EmpresaMasFrecuente { get; set; } = string.Empty;
    public string  PaisOrigenMasFrecuente { get; set; } = string.Empty;
}
