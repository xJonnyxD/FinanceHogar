namespace FinanceHogar.Domain.BusinessRules;

public static class PuntajeFinancieroCalculator
{
    // Score 0-100:
    // Cumplimiento de presupuesto   30 pts
    // Tasa de ahorro                25 pts
    // Puntualidad servicios básicos 20 pts
    // Diversidad de gastos          15 pts
    // Estabilidad de ingresos       10 pts
    public static decimal Calcular(
        decimal totalIngresos,
        decimal totalGastos,
        decimal totalAhorro,
        int categoriasEnPresupuesto,
        int totalCategorias,
        bool serviciosPagadosPuntual,
        decimal ingresoRecurrente,
        bool hayCategoriaDominante)  // true si alguna categoría supera el 40% del gasto total
    {
        if (totalIngresos <= 0) return 0;

        decimal puntosCumplimiento = totalCategorias > 0
            ? Math.Round((decimal)categoriasEnPresupuesto / totalCategorias * 30, 2)
            : 0;

        decimal tasaAhorro = totalAhorro / totalIngresos;
        decimal puntosAhorro = tasaAhorro switch
        {
            >= 0.20m => 25,
            >= 0.10m => 18,
            >= 0.05m => 10,
            > 0m     => 5,
            _        => 0
        };

        decimal puntosServicios = serviciosPagadosPuntual ? 20 : 8;

        decimal puntosDiversidad = hayCategoriaDominante ? 5 : 15;

        decimal tasaRecurrente = ingresoRecurrente / totalIngresos;
        decimal puntosEstabilidad = Math.Round(Math.Min(tasaRecurrente, 1m) * 10, 2);

        return Math.Min(100, puntosCumplimiento + puntosAhorro + puntosServicios +
                             puntosDiversidad + puntosEstabilidad);
    }
}
