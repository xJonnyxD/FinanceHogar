namespace FinanceHogar.Web.Models;

// Alertas
public record AlertaDto(Guid Id, Guid HogarId, string Tipo, string Mensaje, string Estado, DateTime FechaGeneracion);

// Servicios Básicos
public record ServicioBasicoDto(Guid Id, Guid HogarId, string TipoServicio, string NombreProveedor,
    decimal MontoPromedio, DateOnly FechaVencimiento, int DiasAnticipacionNotificacion, bool EstaPagado, DateTime CreatedAt);

public class CreateServicioRequest
{
    public Guid HogarId { get; set; }
    public int TipoServicio { get; set; } = 1;
    public string NombreProveedor { get; set; } = "";
    public decimal MontoPromedio { get; set; }
    public DateOnly FechaVencimiento { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddMonths(1));
    public int DiasAnticipacionNotificacion { get; set; } = 5;
}

public class UpdateServicioRequest
{
    public string NombreProveedor { get; set; } = "";
    public decimal MontoPromedio { get; set; }
    public DateOnly FechaVencimiento { get; set; }
    public int DiasAnticipacionNotificacion { get; set; } = 5;
}

// Tandas
public record TandaDto(Guid Id, Guid HogarId, string Nombre, decimal CuotaMensual,
    int TotalParticipantes, int TurnoActual, string Estado, DateOnly FechaInicio, DateTime CreatedAt);

public class CreateTandaRequest
{
    public Guid HogarId { get; set; }
    public string Nombre { get; set; } = "";
    public decimal CuotaMensual { get; set; }
    public int TotalParticipantes { get; set; } = 2;
    public DateOnly FechaInicio { get; set; } = DateOnly.FromDateTime(DateTime.Today);
}

// Remesas
public record RemesaDto(Guid Id, Guid HogarId, decimal Monto, string Moneda,
    string PaisOrigen, string Empresa, string? NumeroConfirmacion, DateOnly FechaRecepcion, DateTime CreatedAt);

public class CreateRemesaRequest
{
    public Guid HogarId { get; set; }
    public Guid CategoriaId { get; set; }
    public decimal Monto { get; set; }
    public string Moneda { get; set; } = "USD";
    public string PaisOrigen { get; set; } = "Estados Unidos";
    public string Empresa { get; set; } = "";
    public string? NumeroConfirmacion { get; set; }
    public DateOnly FechaRecepcion { get; set; } = DateOnly.FromDateTime(DateTime.Today);
}

// Reportes
public record BalanceMensualDto(decimal TotalIngresos, decimal TotalGastos, decimal Balance, int Anio, int Mes);
public record PuntajeFinancieroDto(int Puntaje, string Nivel, List<string> Recomendaciones);
public record TendenciaDto(int Anio, int Mes, decimal Ingresos, decimal Gastos);
