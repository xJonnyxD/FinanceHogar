using FinanceHogar.Application.DTOs.ServiciosBasicos;
using FinanceHogar.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHogar.API.Controllers;

[Authorize]
public class ServiciosBasicosController : BaseController
{
    private readonly IServiciosBasicosService _svc;
    public ServiciosBasicosController(IServiciosBasicosService svc) => _svc = svc;

    /// <summary>Listar servicios básicos del hogar (ANDA, DELSUR, etc.).</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid hogarId, CancellationToken ct)
    {
        var lista = await _svc.ObtenerPorHogarAsync(hogarId, ct);
        return Ok(lista);
    }

    /// <summary>Obtener servicio básico por ID.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var dto = await _svc.ObtenerPorIdAsync(id, ct);
        return Ok(dto);
    }

    /// <summary>Registrar servicio básico.</summary>
    [HttpPost]
    public async Task<IActionResult> Create(CreateServicioBasicoRequest req, CancellationToken ct)
    {
        var dto = await _svc.CrearAsync(req, UsuarioActualId, ct);
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    /// <summary>Actualizar servicio básico.</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateServicioBasicoRequest req, CancellationToken ct)
    {
        var dto = await _svc.ActualizarAsync(id, req, ct);
        return Ok(dto);
    }

    /// <summary>Eliminar servicio básico.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _svc.EliminarAsync(id, ct);
        return NoContent();
    }

    /// <summary>
    /// Pagar servicio — crea un Gasto automáticamente y avanza la fecha de vencimiento al siguiente mes.
    /// </summary>
    [HttpPost("{id:guid}/pagar")]
    public async Task<IActionResult> Pagar(Guid id, CancellationToken ct)
    {
        var dto = await _svc.PagarAsync(id, UsuarioActualId, ct);
        return Ok(dto);
    }

    /// <summary>Servicios próximos a vencer en los próximos N días.</summary>
    [HttpGet("vencimientos")]
    public async Task<IActionResult> Vencimientos(
        [FromQuery] Guid hogarId, [FromQuery] int dias = 5, CancellationToken ct = default)
    {
        var lista = await _svc.ObtenerVencimientosProximosAsync(hogarId, dias, ct);
        return Ok(lista);
    }
}
