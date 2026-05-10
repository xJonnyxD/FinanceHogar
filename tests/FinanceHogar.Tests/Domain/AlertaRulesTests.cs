using FinanceHogar.Domain.BusinessRules;
using FinanceHogar.Domain.Enums;
using FluentAssertions;

namespace FinanceHogar.Tests.Domain;

public class AlertaRulesTests
{
    // ── EvaluarPresupuesto ─────────────────────────────────────────────────────

    [Fact]
    public void EvaluarPresupuesto_CruceUmbral50_RetornaAlertaCincuenta()
    {
        var resultado = AlertaRules.EvaluarPresupuesto(porcentajeNuevo: 55m, porcentajeAnterior: 40m);

        resultado.Should().Be(TipoAlerta.PresupuestoAlCincuentaPorciento);
    }

    [Fact]
    public void EvaluarPresupuesto_CruceUmbral80_RetornaAlertaOchenta()
    {
        var resultado = AlertaRules.EvaluarPresupuesto(porcentajeNuevo: 85m, porcentajeAnterior: 70m);

        resultado.Should().Be(TipoAlerta.PresupuestoAlOchentaPorciento);
    }

    [Fact]
    public void EvaluarPresupuesto_CruceUmbral100_RetornaAlertaSuperado()
    {
        var resultado = AlertaRules.EvaluarPresupuesto(porcentajeNuevo: 110m, porcentajeAnterior: 90m);

        resultado.Should().Be(TipoAlerta.PresupuestoSuperado);
    }

    [Fact]
    public void EvaluarPresupuesto_SinCruce_RetornaNull()
    {
        var resultado = AlertaRules.EvaluarPresupuesto(porcentajeNuevo: 45m, porcentajeAnterior: 30m);

        resultado.Should().BeNull();
    }

    [Fact]
    public void EvaluarPresupuesto_YaSuperadoAntes_RetornaNull()
    {
        // Ya estaba por encima del 100% — no debe disparar alerta duplicada
        var resultado = AlertaRules.EvaluarPresupuesto(porcentajeNuevo: 120m, porcentajeAnterior: 105m);

        resultado.Should().BeNull();
    }

    [Fact]
    public void EvaluarPresupuesto_ExactamenteEnUmbral50_RetornaAlertaCincuenta()
    {
        var resultado = AlertaRules.EvaluarPresupuesto(porcentajeNuevo: 50m, porcentajeAnterior: 49m);

        resultado.Should().Be(TipoAlerta.PresupuestoAlCincuentaPorciento);
    }

    [Fact]
    public void EvaluarPresupuesto_ExactamenteEnUmbral80_RetornaAlertaOchenta()
    {
        var resultado = AlertaRules.EvaluarPresupuesto(porcentajeNuevo: 80m, porcentajeAnterior: 79m);

        resultado.Should().Be(TipoAlerta.PresupuestoAlOchentaPorciento);
    }

    [Fact]
    public void EvaluarPresupuesto_ExactamenteEnUmbral100_RetornaAlertaSuperado()
    {
        var resultado = AlertaRules.EvaluarPresupuesto(porcentajeNuevo: 100m, porcentajeAnterior: 99m);

        resultado.Should().Be(TipoAlerta.PresupuestoSuperado);
    }

    // ── DebeNotificarServicioBasico ────────────────────────────────────────────

    [Fact]
    public void DebeNotificarServicioBasico_VenceEnUnDia_RetornaTrue()
    {
        var fechaVencimiento = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(1));

        var resultado = AlertaRules.DebeNotificarServicioBasico(fechaVencimiento, diasAnticipacion: 5);

        resultado.Should().BeTrue();
    }

    [Fact]
    public void DebeNotificarServicioBasico_VenceEnDiezDias_FueraDePlazo_RetornaFalse()
    {
        var fechaVencimiento = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(10));

        var resultado = AlertaRules.DebeNotificarServicioBasico(fechaVencimiento, diasAnticipacion: 5);

        resultado.Should().BeFalse();
    }

    [Fact]
    public void DebeNotificarServicioBasico_YaVencido_RetornaFalse()
    {
        var fechaVencimiento = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));

        var resultado = AlertaRules.DebeNotificarServicioBasico(fechaVencimiento, diasAnticipacion: 5);

        resultado.Should().BeFalse();
    }

    // ── Métodos de temporada ───────────────────────────────────────────────────

    [Theory]
    [InlineData(1)]
    [InlineData(7)]
    public void EsTemporadaEscolar_MesesValidos_RetornaTrue(int mes)
    {
        AlertaRules.EsTemporadaEscolar(mes).Should().BeTrue();
    }

    [Theory]
    [InlineData(2), InlineData(5), InlineData(12)]
    public void EsTemporadaEscolar_MesesNoValidos_RetornaFalse(int mes)
    {
        AlertaRules.EsTemporadaEscolar(mes).Should().BeFalse();
    }

    [Fact]
    public void EsTemporadaNavidad_Noviembre_RetornaTrue()
    {
        AlertaRules.EsTemporadaNavidad(11).Should().BeTrue();
    }

    [Theory]
    [InlineData(10), InlineData(12)]
    public void EsTemporadaNavidad_OtrosMeses_RetornaFalse(int mes)
    {
        AlertaRules.EsTemporadaNavidad(mes).Should().BeFalse();
    }
}
