using FinanceHogar.Application.DTOs.Gastos;
using FinanceHogar.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHogar.API.Controllers;

[Authorize]
public class GastosController : BaseController
{
    private readonly IGastosService _svc;
    public GastosController(IGastosService svc) => _svc = svc;

    /// <summary>Listar gastos del hogar con filtros opcionales.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid hogarId,
        [FromQuery] DateOnly? desde,
        [FromQuery] DateOnly? hasta,
        [FromQuery] Guid? categoriaId,
        CancellationToken ct)
    {
        var lista = await _svc.ObtenerPorHogarAsync(hogarId, desde, hasta, categoriaId, ct);
        return Ok(lista);
    }

    /// <summary>Obtener gasto por ID.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var dto = await _svc.ObtenerPorIdAsync(id, ct);
        return Ok(dto);
    }

    /// <summary>
    /// Registrar gasto. Si supera un umbral de presupuesto (50/80/100%)
    /// la respuesta incluye el campo <c>alertaGenerada</c>.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(CreateGastoRequest req, CancellationToken ct)
    {
        var dto = await _svc.RegistrarAsync(req, UsuarioActualId, ct);
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    /// <summary>Actualizar gasto.</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateGastoRequest req, CancellationToken ct)
    {
        var dto = await _svc.ActualizarAsync(id, req, UsuarioActualId, ct);
        return Ok(dto);
    }

    /// <summary>Eliminar gasto (soft delete).</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _svc.EliminarAsync(id, UsuarioActualId, ct);
        return NoContent();
    }

    /// <summary>Gastos recurrentes del hogar.</summary>
    [HttpGet("recurrentes")]
    public async Task<IActionResult> Recurrentes([FromQuery] Guid hogarId, CancellationToken ct)
    {
        var lista = await _svc.ObtenerRecurrentesAsync(hogarId, ct);
        return Ok(lista);
    }
}
