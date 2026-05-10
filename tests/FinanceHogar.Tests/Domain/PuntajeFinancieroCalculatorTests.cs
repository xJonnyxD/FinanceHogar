using FinanceHogar.Domain.BusinessRules;
using FluentAssertions;

namespace FinanceHogar.Tests.Domain;

public class PuntajeFinancieroCalculatorTests
{
    // ── Sin ingresos ───────────────────────────────────────────────────────────

    [Fact]
    public void Calcular_SinIngresos_RetornaCero()
    {
        var puntaje = PuntajeFinancieroCalculator.Calcular(
            totalIngresos: 0, totalGastos: 0, totalAhorro: 0,
            categoriasEnPresupuesto: 0, totalCategorias: 0,
            serviciosPagadosPuntual: true, ingresoRecurrente: 0,
            hayCategoriaDominante: false);

        puntaje.Should().Be(0);
    }

    // ── Ahorro alto (≥ 20%) → 25 pts ──────────────────────────────────────────

    [Fact]
    public void Calcular_AhorroAltoYTodoEnPresupuesto_RetornaPuntajeAlto()
    {
        // Ahorro 20% → 25 pts | Presupuesto 100% → 30 pts | Servicios OK → 20 pts
        // Diversidad OK → 15 pts | Ingreso rec 100% → 10 pts = 100 pts
        var puntaje = PuntajeFinancieroCalculator.Calcular(
            totalIngresos: 1000m, totalGastos: 800m, totalAhorro: 200m,
            categoriasEnPresupuesto: 5, totalCategorias: 5,
            serviciosPagadosPuntual: true, ingresoRecurrente: 1000m,
            hayCategoriaDominante: false);

        puntaje.Should().Be(100m);
    }

    [Fact]
    public void Calcular_AhorroMedio_RetornaEighteenPuntosAhorro()
    {
        // Ahorro 10% → 18 pts ahorro; el resto en condición base para aislar
        var puntaje = PuntajeFinancieroCalculator.Calcular(
            totalIngresos: 1000m, totalGastos: 900m, totalAhorro: 100m,
            categoriasEnPresupuesto: 0, totalCategorias: 0,
            serviciosPagadosPuntual: true, ingresoRecurrente: 0m,
            hayCategoriaDominante: false);

        // 18 pts ahorro + 20 pts servicios + 15 pts diversidad + 0 estabilidad + 0 presupuesto
        puntaje.Should().Be(53m);
    }

    [Fact]
    public void Calcular_AhorroBajo_RetornaCincoPuntosAhorro()
    {
        // Ahorro 5% exacto → 10 pts
        var puntaje = PuntajeFinancieroCalculator.Calcular(
            totalIngresos: 1000m, totalGastos: 950m, totalAhorro: 50m,
            categoriasEnPresupuesto: 0, totalCategorias: 0,
            serviciosPagadosPuntual: true, ingresoRecurrente: 0m,
            hayCategoriaDominante: false);

        // 10 pts ahorro + 20 servicios + 15 diversidad = 45
        puntaje.Should().Be(45m);
    }

    // ── Servicios vencidos ─────────────────────────────────────────────────────

    [Fact]
    public void Calcular_ServiciosVencidos_DescuentaDoceDeVeinteEnServicios()
    {
        var puntaje = PuntajeFinancieroCalculator.Calcular(
            totalIngresos: 1000m, totalGastos: 800m, totalAhorro: 200m,
            categoriasEnPresupuesto: 5, totalCategorias: 5,
            serviciosPagadosPuntual: false, ingresoRecurrente: 1000m,
            hayCategoriaDominante: false);

        // 30 + 25 + 8 (servicios vencidos) + 15 + 10 = 88
        puntaje.Should().Be(88m);
    }

    // ── Categoría dominante ────────────────────────────────────────────────────

    [Fact]
    public void Calcular_CategoriaDominante_ReduceDiversidadAcinco()
    {
        var puntaje = PuntajeFinancieroCalculator.Calcular(
            totalIngresos: 1000m, totalGastos: 800m, totalAhorro: 200m,
            categoriasEnPresupuesto: 5, totalCategorias: 5,
            serviciosPagadosPuntual: true, ingresoRecurrente: 1000m,
            hayCategoriaDominante: true);

        // 30 + 25 + 20 + 5 (dominante) + 10 = 90
        puntaje.Should().Be(90m);
    }

    // ── Cumplimiento de presupuesto parcial ────────────────────────────────────

    [Fact]
    public void Calcular_MitadDeCategoriasEnPresupuesto_RetornequincePuntosCumplimiento()
    {
        var puntaje = PuntajeFinancieroCalculator.Calcular(
            totalIngresos: 1000m, totalGastos: 800m, totalAhorro: 200m,
            categoriasEnPresupuesto: 2, totalCategorias: 4,
            serviciosPagadosPuntual: true, ingresoRecurrente: 1000m,
            hayCategoriaDominante: false);

        // 15 presupuesto + 25 ahorro + 20 servicios + 15 diversidad + 10 estabilidad = 85
        puntaje.Should().Be(85m);
    }

    // ── Puntaje no supera 100 ──────────────────────────────────────────────────

    [Fact]
    public void Calcular_CondicionesIdeales_NuncaSuperaCien()
    {
        var puntaje = PuntajeFinancieroCalculator.Calcular(
            totalIngresos: 5000m, totalGastos: 1000m, totalAhorro: 4000m,
            categoriasEnPresupuesto: 10, totalCategorias: 10,
            serviciosPagadosPuntual: true, ingresoRecurrente: 5000m,
            hayCategoriaDominante: false);

        puntaje.Should().BeLessThanOrEqualTo(100m);
    }

    // ── Estabilidad de ingreso recurrente ──────────────────────────────────────

    [Fact]
    public void Calcular_SinIngresoRecurrente_CeroPuntosEstabilidad()
    {
        var conRecurrente = PuntajeFinancieroCalculator.Calcular(
            totalIngresos: 1000m, totalGastos: 800m, totalAhorro: 200m,
            categoriasEnPresupuesto: 5, totalCategorias: 5,
            serviciosPagadosPuntual: true, ingresoRecurrente: 1000m,
            hayCategoriaDominante: false);

        var sinRecurrente = PuntajeFinancieroCalculator.Calcular(
            totalIngresos: 1000m, totalGastos: 800m, totalAhorro: 200m,
            categoriasEnPresupuesto: 5, totalCategorias: 5,
            serviciosPagadosPuntual: true, ingresoRecurrente: 0m,
            hayCategoriaDominante: false);

        conRecurrente.Should().BeGreaterThan(sinRecurrente);
        (conRecurrente - sinRecurrente).Should().Be(10m);
    }
}
