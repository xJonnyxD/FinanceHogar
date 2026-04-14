using FinanceHogar.Application.Interfaces;
using FinanceHogar.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHogar.API.Controllers;

[Authorize]
public class AlertasController : BaseController
{
    private readonly IAlertasService _svc;
    public AlertasController(IAlertasService svc) => _svc = svc;

    /// <summary>Listar alertas del hogar filtradas por estado (Pendiente/Leida/Descartada).</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid hogarId,
        [FromQuery] string? estado,
        CancellationToken ct)
    {
        EstadoAlerta? estadoEnum = null;
        if (!string.IsNullOrWhiteSpace(estado) && Enum.TryParse<EstadoAlerta>(estado, true, out var e))
            estadoEnum = e;

        var lista = await _svc.ObtenerPorHogarAsync(hogarId, estadoEnum, ct);
        return Ok(lista);
    }

    /// <summary>Obtener alerta por ID.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var dto = await _svc.ObtenerPorIdAsync(id, ct);
        return Ok(dto);
    }

    /// <summary>Conteo de alertas no leídas.</summary>
    [HttpGet("no-leidas/count")]
    public async Task<IActionResult> CountNoLeidas([FromQuery] Guid hogarId, CancellationToken ct)
    {
        var count = await _svc.ContarNoLeidasAsync(hogarId, ct);
        return Ok(new { count });
    }

    /// <summary>Marcar alerta como leída.</summary>
    [HttpPut("{id:guid}/leer")]
    public async Task<IActionResult> MarcarLeida(Guid id, CancellationToken ct)
    {
        var dto = await _svc.MarcarComoLeidaAsync(id, ct);
        return Ok(dto);
    }

    /// <summary>Descartar alerta.</summary>
    [HttpPut("{id:guid}/descartar")]
    public async Task<IActionResult> Descartar(Guid id, CancellationToken ct)
    {
        var dto = await _svc.DescartarAsync(id, ct);
        return Ok(dto);
    }

    /// <summary>Eliminar alerta.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _svc.EliminarAsync(id, ct);
        return NoContent();
    }

    /// <summary>Generar alertas de temporada manualmente (admin).</summary>
    [HttpPost("generar")]
    public async Task<IActionResult> GenerarTemporada([FromQuery] Guid hogarId, CancellationToken ct)
    {
        await _svc.GenerarAlertasTemporadaAsync(hogarId, ct);
        return NoContent();
    }
}
