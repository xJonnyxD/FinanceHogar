using FinanceHogar.Application.Interfaces.Repositories;
using FinanceHogar.Application.Services;
using FinanceHogar.Domain.Entities;
using FinanceHogar.Domain.Enums;
using FluentAssertions;
using Moq;

namespace FinanceHogar.Tests.Application;

public class AlertasServiceTests
{
    private readonly Mock<IAlertasRepository> _alertasRepo = new();
    private readonly Mock<IHogaresRepository> _hogaresRepo = new();

    private AlertasService CrearServicio() =>
        new(_alertasRepo.Object, _hogaresRepo.Object);

    // ── ObtenerPorIdAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task ObtenerPorIdAsync_AlertaExiste_RetornaDto()
    {
        var id = Guid.NewGuid();
        var alerta = new Alerta
        {
            Id    = id,
            HogarId = Guid.NewGuid(),
            Tipo    = TipoAlerta.PresupuestoAlOchentaPorciento,
            Estado  = EstadoAlerta.Pendiente,
            Titulo  = "Alerta 80%",
            Mensaje = "Llevas el 80%"
        };
        _alertasRepo.Setup(r => r.ObtenerPorIdAsync(id, default)).ReturnsAsync(alerta);

        var resultado = await CrearServicio().ObtenerPorIdAsync(id, default);

        resultado.Id.Should().Be(id);
        resultado.Tipo.Should().Be("PresupuestoAlOchentaPorciento");
    }

    [Fact]
    public async Task ObtenerPorIdAsync_AlertaNoExiste_LanzaKeyNotFoundException()
    {
        var id = Guid.NewGuid();
        _alertasRepo.Setup(r => r.ObtenerPorIdAsync(id, default)).ReturnsAsync((Alerta?)null);

        var act = async () => await CrearServicio().ObtenerPorIdAsync(id, default);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*{id}*");
    }

    // ── MarcarComoLeidaAsync ───────────────────────────────────────────────────

    [Fact]
    public async Task MarcarComoLeidaAsync_AlertaPendiente_CambiaEstadoALeida()
    {
        var id = Guid.NewGuid();
        var alerta = new Alerta
        {
            Id      = id,
            HogarId = Guid.NewGuid(),
            Estado  = EstadoAlerta.Pendiente,
            Tipo    = TipoAlerta.PresupuestoAlCincuentaPorciento,
            Titulo  = "50%",
            Mensaje = "Mensaje"
        };
        _alertasRepo.Setup(r => r.ObtenerPorIdAsync(id, default)).ReturnsAsync(alerta);
        _alertasRepo.Setup(r => r.ActualizarAsync(It.IsAny<Alerta>(), default))
            .ReturnsAsync(alerta);

        var resultado = await CrearServicio().MarcarComoLeidaAsync(id, default);

        resultado.Estado.Should().Be("Leida");
        alerta.FechaLeida.Should().NotBeNull();
    }

    [Fact]
    public async Task MarcarComoLeidaAsync_AlertaNoExiste_LanzaKeyNotFoundException()
    {
        var id = Guid.NewGuid();
        _alertasRepo.Setup(r => r.ObtenerPorIdAsync(id, default)).ReturnsAsync((Alerta?)null);

        var act = async () => await CrearServicio().MarcarComoLeidaAsync(id, default);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    // ── DescartarAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task DescartarAsync_AlertaExiste_CambiaEstadoADescartada()
    {
        var id = Guid.NewGuid();
        var alerta = new Alerta
        {
            Id      = id,
            HogarId = Guid.NewGuid(),
            Estado  = EstadoAlerta.Pendiente,
            Tipo    = TipoAlerta.PresupuestoSuperado,
            Titulo  = "Superado",
            Mensaje = "Mensaje"
        };
        _alertasRepo.Setup(r => r.ObtenerPorIdAsync(id, default)).ReturnsAsync(alerta);
        _alertasRepo.Setup(r => r.ActualizarAsync(It.IsAny<Alerta>(), default))
            .ReturnsAsync(alerta);

        var resultado = await CrearServicio().DescartarAsync(id, default);

        resultado.Estado.Should().Be("Descartada");
    }

    [Fact]
    public async Task DescartarAsync_AlertaNoExiste_LanzaKeyNotFoundException()
    {
        var id = Guid.NewGuid();
        _alertasRepo.Setup(r => r.ObtenerPorIdAsync(id, default)).ReturnsAsync((Alerta?)null);

        var act = async () => await CrearServicio().DescartarAsync(id, default);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    // ── EliminarAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task EliminarAsync_AlertaNoExiste_LanzaKeyNotFoundException()
    {
        var id = Guid.NewGuid();
        _alertasRepo.Setup(r => r.ObtenerPorIdAsync(id, default)).ReturnsAsync((Alerta?)null);

        var act = async () => await CrearServicio().EliminarAsync(id, default);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task EliminarAsync_AlertaExiste_LlamaRepositorioEliminar()
    {
        var id = Guid.NewGuid();
        var alerta = new Alerta
        {
            Id = id, HogarId = Guid.NewGuid(), Estado = EstadoAlerta.Leida,
            Tipo = TipoAlerta.PresupuestoAlCincuentaPorciento, Titulo = "T", Mensaje = "M"
        };
        _alertasRepo.Setup(r => r.ObtenerPorIdAsync(id, default)).ReturnsAsync(alerta);
        _alertasRepo.Setup(r => r.EliminarAsync(id, default)).Returns(Task.CompletedTask);

        await CrearServicio().EliminarAsync(id, default);

        _alertasRepo.Verify(r => r.EliminarAsync(id, default), Times.Once);
    }

    // ── ContarNoLeidasAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task ContarNoLeidasAsync_RetornaCantidadDelRepositorio()
    {
        var hogarId = Guid.NewGuid();
        _alertasRepo.Setup(r => r.ContarNoLeidasAsync(hogarId, default)).ReturnsAsync(3);

        var resultado = await CrearServicio().ContarNoLeidasAsync(hogarId, default);

        resultado.Should().Be(3);
    }
}
