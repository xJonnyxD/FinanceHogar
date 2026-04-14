using FinanceHogar.Domain.Enums;

namespace FinanceHogar.Domain.BusinessRules;

public static class AlertaRules
{
    public const decimal UmbralAdvertencia = 50m;
    public const decimal UmbralAlerta = 80m;
    public const decimal UmbralSuperado = 100m;

    // Solo dispara una alerta al cruzar un umbral, no en cada guardado
    public static TipoAlerta? EvaluarPresupuesto(decimal porcentajeNuevo, decimal porcentajeAnterior)
    {
        return (porcentajeAnterior, porcentajeNuevo) switch
        {
            var (prev, curr) when prev < UmbralSuperado  && curr >= UmbralSuperado  => TipoAlerta.PresupuestoSuperado,
            var (prev, curr) when prev < UmbralAlerta    && curr >= UmbralAlerta    => TipoAlerta.PresupuestoAlOchentaPorciento,
            var (prev, curr) when prev < UmbralAdvertencia && curr >= UmbralAdvertencia => TipoAlerta.PresupuestoAlCincuentaPorciento,
            _ => null
        };
    }

    public static bool DebeNotificarServicioBasico(DateOnly fechaVencimiento, int diasAnticipacion)
    {
        var diasRestantes = (fechaVencimiento.ToDateTime(TimeOnly.MinValue) - DateTime.UtcNow.Date).Days;
        return diasRestantes >= 0 && diasRestantes <= diasAnticipacion;
    }

    public static bool EsTemporadaEscolar(int mes) => mes == 1 || mes == 7;  // Enero y Julio
    public static bool EsTemporadaNavidad(int mes) => mes == 11;              // Noviembre — anticipa diciembre
}
