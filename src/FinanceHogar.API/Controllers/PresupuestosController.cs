using FinanceHogar.Application.DTOs.Presupuestos;
using FinanceHogar.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHogar.API.Controllers;

[Authorize]
public class PresupuestosController : BaseController
{
    private readonly IPresupuestosService _svc;
    public PresupuestosController(IPresupuestosService svc) => _svc = svc;

    /// <summary>Listar presupuestos del hogar para un mes.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid hogarId, [FromQuery] int anio, [FromQuery] int mes, CancellationToken ct)
    {
        var lista = await _svc.ObtenerPorHogarYMesAsync(hogarId, anio, mes, ct);
        return Ok(lista);
    }

    /// <summary>Obtener presupuesto por ID.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var dto = await _svc.ObtenerPorIdAsync(id, ct);
        return Ok(dto);
    }

    /// <summary>Crear presupuesto mensual para una categoría.</summary>
    [HttpPost]
    public async Task<IActionResult> Create(CreatePresupuestoRequest req, CancellationToken ct)
    {
        var dto = await _svc.CrearAsync(req, ct);
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    /// <summary>Actualizar monto límite del presupuesto.</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdatePresupuestoRequest req, CancellationToken ct)
    {
        var dto = await _svc.ActualizarAsync(id, req, ct);
        return Ok(dto);
    }

    /// <summary>Eliminar presupuesto.</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _svc.EliminarAsync(id, ct);
        return NoContent();
    }

    /// <summary>Comparativo presupuestado vs real para el mes.</summary>
    [HttpGet("vs-real")]
    public async Task<IActionResult> VsReal(
        [FromQuery] Guid hogarId, [FromQuery] int anio, [FromQuery] int mes, CancellationToken ct)
    {
        var lista = await _svc.ObtenerVsRealAsync(hogarId, anio, mes, ct);
        return Ok(lista);
    }

    /// <summary>Copiar presupuestos de un mes a otro.</summary>
    [HttpPost("copiar")]
    public async Task<IActionResult> Copiar(CopiarPresupuestoRequest req, CancellationToken ct)
    {
        var lista = await _svc.CopiarMesAnteriorAsync(req, ct);
        return Ok(lista);
    }
}
