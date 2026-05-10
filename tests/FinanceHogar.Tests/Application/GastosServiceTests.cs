using FinanceHogar.Application.DTOs.Gastos;
using FinanceHogar.Application.Interfaces.Repositories;
using FinanceHogar.Application.Services;
using FinanceHogar.Domain.Entities;
using FinanceHogar.Domain.Enums;
using FluentAssertions;
using Moq;

namespace FinanceHogar.Tests.Application;

public class GastosServiceTests
{
    private readonly Mock<IGastosRepository> _gastosRepo = new();
    private readonly Mock<IHogaresRepository> _hogaresRepo = new();
    private readonly Mock<IPresupuestosRepository> _presupuestosRepo = new();
    private readonly Mock<IAlertasRepository> _alertasRepo = new();

    private GastosService CrearServicio() =>
        new(_gastosRepo.Object, _hogaresRepo.Object, _presupuestosRepo.Object, _alertasRepo.Object);

    // ── ObtenerPorIdAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task ObtenerPorIdAsync_GastoExiste_RetornaDto()
    {
        var id = Guid.NewGuid();
        var gasto = new Gasto
        {
            Id          = id,
            UsuarioId   = Guid.NewGuid(),
            HogarId     = Guid.NewGuid(),
            CategoriaId = Guid.NewGuid(),
            Monto       = 50m,
            Moneda      = TipoMoneda.USD,
            MontoEnUSD  = 50m,
            Tipo        = TipoGasto.Otro,
            FechaGasto  = DateOnly.FromDateTime(DateTime.UtcNow)
        };
        _gastosRepo.Setup(r => r.ObtenerPorIdAsync(id, default)).ReturnsAsync(gasto);

        var resultado = await CrearServicio().ObtenerPorIdAsync(id, default);

        resultado.Id.Should().Be(id);
        resultado.Monto.Should().Be(50m);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_GastoNoExiste_LanzaKeyNotFoundException()
    {
        var id = Guid.NewGuid();
        _gastosRepo.Setup(r => r.ObtenerPorIdAsync(id, default)).ReturnsAsync((Gasto?)null);

        var act = async () => await CrearServicio().ObtenerPorIdAsync(id, default);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*{id}*");
    }

    // ── RegistrarAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task RegistrarAsync_HogarNoExiste_LanzaKeyNotFoundException()
    {
        var req = new CreateGastoRequest
        {
            HogarId     = Guid.NewGuid(),
            CategoriaId = Guid.NewGuid(),
            Monto       = 100m,
            Moneda      = TipoMoneda.USD,
            FechaGasto  = DateOnly.FromDateTime(DateTime.UtcNow)
        };
        _hogaresRepo.Setup(r => r.ExisteAsync(req.HogarId, default)).ReturnsAsync(false);

        var act = async () => await CrearServicio().RegistrarAsync(req, Guid.NewGuid(), default);

        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"*{req.HogarId}*");
    }

    [Fact]
    public async Task RegistrarAsync_SinPresupuesto_CreaGastoSinGenerarAlerta()
    {
        var hogarId     = Guid.NewGuid();
        var categoriaId = Guid.NewGuid();
        var usuarioId   = Guid.NewGuid();
        var req = new CreateGastoRequest
        {
            HogarId     = hogarId,
            CategoriaId = categoriaId,
            Monto       = 100m,
            Moneda      = TipoMoneda.USD,
            FechaGasto  = DateOnly.FromDateTime(DateTime.UtcNow)
        };
        var gastoCreado = new Gasto
        {
            Id = Guid.NewGuid(), HogarId = hogarId, CategoriaId = categoriaId,
            UsuarioId = usuarioId, Monto = 100m, Moneda = TipoMoneda.USD, MontoEnUSD = 100m,
            Tipo = TipoGasto.Otro, FechaGasto = req.FechaGasto
        };

        _hogaresRepo.Setup(r => r.ExisteAsync(hogarId, default)).ReturnsAsync(true);
        _gastosRepo.Setup(r => r.AgregarAsync(It.IsAny<Gasto>(), default)).ReturnsAsync(gastoCreado);
        _presupuestosRepo.Setup(r => r.ObtenerPorHogarCategoriaYMesAsync(
            hogarId, categoriaId, It.IsAny<int>(), It.IsAny<int>(), default))
            .ReturnsAsync((PresupuestoMensual?)null);

        var resultado = await CrearServicio().RegistrarAsync(req, usuarioId, default);

        resultado.Id.Should().Be(gastoCreado.Id);
        resultado.AlertaGenerada.Should().BeNull();
        _alertasRepo.Verify(r => r.AgregarAsync(It.IsAny<Alerta>(), default), Times.Never);
    }

    [Fact]
    public async Task RegistrarAsync_CruzaUmbral80_GeneraAlertaOchentaPorciento()
    {
        var hogarId     = Guid.NewGuid();
        var categoriaId = Guid.NewGuid();
        var usuarioId   = Guid.NewGuid();

        var presupuesto = new PresupuestoMensual
        {
            HogarId     = hogarId,
            CategoriaId = categoriaId,
            MontoLimite = 100m,
            MontoGastado = 70m   // 70% antes del gasto → tras sumar 15 queda en 85% → cruza 80%
        };
        var req = new CreateGastoRequest
        {
            HogarId     = hogarId,
            CategoriaId = categoriaId,
            Monto       = 15m,
            Moneda      = TipoMoneda.USD,
            FechaGasto  = DateOnly.FromDateTime(DateTime.UtcNow)
        };
        var gastoCreado = new Gasto
        {
            Id = Guid.NewGuid(), HogarId = hogarId, CategoriaId = categoriaId,
            UsuarioId = usuarioId, Monto = 15m, Moneda = TipoMoneda.USD, MontoEnUSD = 15m,
            Tipo = TipoGasto.Otro, FechaGasto = req.FechaGasto
        };

        _hogaresRepo.Setup(r => r.ExisteAsync(hogarId, default)).ReturnsAsync(true);
        _gastosRepo.Setup(r => r.AgregarAsync(It.IsAny<Gasto>(), default)).ReturnsAsync(gastoCreado);
        _presupuestosRepo.Setup(r => r.ObtenerPorHogarCategoriaYMesAsync(
            hogarId, categoriaId, It.IsAny<int>(), It.IsAny<int>(), default))
            .ReturnsAsync(presupuesto);
        _presupuestosRepo.Setup(r => r.ActualizarAsync(It.IsAny<PresupuestoMensual>(), default))
            .ReturnsAsync(presupuesto);
        _alertasRepo.Setup(r => r.AgregarAsync(It.IsAny<Alerta>(), default))
            .ReturnsAsync(new Alerta());

        var resultado = await CrearServicio().RegistrarAsync(req, usuarioId, default);

        resultado.AlertaGenerada.Should().NotBeNull();
        resultado.AlertaGenerada!.Tipo.Should().Be("PresupuestoAlOchentaPorciento");
        _alertasRepo.Verify(r => r.AgregarAsync(It.IsAny<Alerta>(), default), Times.Once);
    }

    [Fact]
    public async Task RegistrarAsync_CruzaUmbral100_GeneraAlertaSuperado()
    {
        var hogarId     = Guid.NewGuid();
        var categoriaId = Guid.NewGuid();
        var usuarioId   = Guid.NewGuid();

        var presupuesto = new PresupuestoMensual
        {
            HogarId      = hogarId,
            CategoriaId  = categoriaId,
            MontoLimite  = 100m,
            MontoGastado = 90m   // 90% → tras sumar 15 queda en 105% → cruza 100%
        };
        var req = new CreateGastoRequest
        {
            HogarId = hogarId, CategoriaId = categoriaId,
            Monto = 15m, Moneda = TipoMoneda.USD,
            FechaGasto = DateOnly.FromDateTime(DateTime.UtcNow)
        };
        var gastoCreado = new Gasto
        {
            Id = Guid.NewGuid(), HogarId = hogarId, CategoriaId = categoriaId,
            UsuarioId = usuarioId, Monto = 15m, Moneda = TipoMoneda.USD, MontoEnUSD = 15m,
            Tipo = TipoGasto.Otro, FechaGasto = req.FechaGasto
        };

        _hogaresRepo.Setup(r => r.ExisteAsync(hogarId, default)).ReturnsAsync(true);
        _gastosRepo.Setup(r => r.AgregarAsync(It.IsAny<Gasto>(), default)).ReturnsAsync(gastoCreado);
        _presupuestosRepo.Setup(r => r.ObtenerPorHogarCategoriaYMesAsync(
            hogarId, categoriaId, It.IsAny<int>(), It.IsAny<int>(), default))
            .ReturnsAsync(presupuesto);
        _presupuestosRepo.Setup(r => r.ActualizarAsync(It.IsAny<PresupuestoMensual>(), default))
            .ReturnsAsync(presupuesto);
        _alertasRepo.Setup(r => r.AgregarAsync(It.IsAny<Alerta>(), default))
            .ReturnsAsync(new Alerta());

        var resultado = await CrearServicio().RegistrarAsync(req, usuarioId, default);

        resultado.AlertaGenerada!.Tipo.Should().Be("PresupuestoSuperado");
    }

    // ── EliminarAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task EliminarAsync_GastoNoExiste_LanzaKeyNotFoundException()
    {
        var id = Guid.NewGuid();
        _gastosRepo.Setup(r => r.ObtenerPorIdAsync(id, default)).ReturnsAsync((Gasto?)null);

        var act = async () => await CrearServicio().EliminarAsync(id, Guid.NewGuid(), default);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task EliminarAsync_UsuarioNoEsDueno_LanzaUnauthorizedAccessException()
    {
        var id        = Guid.NewGuid();
        var dueno     = Guid.NewGuid();
        var otro      = Guid.NewGuid();
        var gasto = new Gasto
        {
            Id = id, UsuarioId = dueno, HogarId = Guid.NewGuid(),
            CategoriaId = Guid.NewGuid(), Monto = 50m, FechaGasto = DateOnly.FromDateTime(DateTime.UtcNow)
        };
        _gastosRepo.Setup(r => r.ObtenerPorIdAsync(id, default)).ReturnsAsync(gasto);

        var act = async () => await CrearServicio().EliminarAsync(id, otro, default);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task EliminarAsync_UsuarioEsDueno_EliminaExitosamente()
    {
        var id      = Guid.NewGuid();
        var dueno   = Guid.NewGuid();
        var gasto = new Gasto
        {
            Id = id, UsuarioId = dueno, HogarId = Guid.NewGuid(),
            CategoriaId = Guid.NewGuid(), Monto = 50m, FechaGasto = DateOnly.FromDateTime(DateTime.UtcNow)
        };
        _gastosRepo.Setup(r => r.ObtenerPorIdAsync(id, default)).ReturnsAsync(gasto);
        _gastosRepo.Setup(r => r.EliminarAsync(id, default)).Returns(Task.CompletedTask);

        var act = async () => await CrearServicio().EliminarAsync(id, dueno, default);

        await act.Should().NotThrowAsync();
        _gastosRepo.Verify(r => r.EliminarAsync(id, default), Times.Once);
    }

    // ── ActualizarAsync ────────────────────────────────────────────────────────

    [Fact]
    public async Task ActualizarAsync_UsuarioNoEsDueno_LanzaUnauthorizedAccessException()
    {
        var id    = Guid.NewGuid();
        var dueno = Guid.NewGuid();
        var otro  = Guid.NewGuid();
        var gasto = new Gasto
        {
            Id = id, UsuarioId = dueno, HogarId = Guid.NewGuid(),
            CategoriaId = Guid.NewGuid(), Monto = 50m, FechaGasto = DateOnly.FromDateTime(DateTime.UtcNow)
        };
        _gastosRepo.Setup(r => r.ObtenerPorIdAsync(id, default)).ReturnsAsync(gasto);

        var act = async () => await CrearServicio().ActualizarAsync(id, new UpdateGastoRequest
        {
            CategoriaId = Guid.NewGuid(), Monto = 60m, Moneda = TipoMoneda.USD,
            Tipo = TipoGasto.Otro, FechaGasto = DateOnly.FromDateTime(DateTime.UtcNow)
        }, otro, default);

        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }
}
