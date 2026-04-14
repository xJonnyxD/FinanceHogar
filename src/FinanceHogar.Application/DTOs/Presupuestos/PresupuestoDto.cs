namespace FinanceHogar.Application.DTOs.Presupuestos;

public class PresupuestoDto
{
    public Guid    Id              { get; set; }
    public Guid    HogarId         { get; set; }
    public Guid    CategoriaId     { get; set; }
    public string  NombreCategoria { get; set; } = string.Empty;
    public int     Anio            { get; set; }
    public int     Mes             { get; set; }
    public decimal MontoLimite     { get; set; }
    public decimal MontoGastado    { get; set; }
    public decimal PorcentajeUso   { get; set; }
    public DateTime CreatedAt      { get; set; }
}

public class CreatePresupuestoRequest
{
    public Guid    HogarId     { get; set; }
    public Guid    CategoriaId { get; set; }
    public int     Anio        { get; set; }
    public int     Mes         { get; set; }
    public decimal MontoLimite { get; set; }
}

public class UpdatePresupuestoRequest
{
    public decimal MontoLimite { get; set; }
}

public class CopiarPresupuestoRequest
{
    public Guid HogarId     { get; set; }
    public int  AnioOrigen  { get; set; }
    public int  MesOrigen   { get; set; }
    public int  AnioDestino { get; set; }
    public int  MesDestino  { get; set; }
}

public class PresupuestoVsRealDto
{
    public Guid    CategoriaId     { get; set; }
    public string  NombreCategoria { get; set; } = string.Empty;
    public decimal MontoLimite     { get; set; }
    public decimal MontoGastado    { get; set; }
    public decimal PorcentajeUso   { get; set; }
    public decimal Disponible      { get; set; }
}
