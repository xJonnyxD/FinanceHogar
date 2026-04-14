using FinanceHogar.Application.DTOs.Ingresos;
using FinanceHogar.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHogar.API.Controllers;

[Authorize]
public class IngresosController : BaseController
{
    private readonly IIngresosService _svc;
    public IngresosController(IIngresosService svc) => _svc = svc;

    /// <summary>Listar ingresos del hogar con filtros opcionales.</summary>
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

    /// <summary>Obtener ingreso por ID.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var dto = await _svc.ObtenerPorIdAsync(id, ct);
        return Ok(dto);
    }

    /// <summary>Registrar nuevo ingreso.</summary>
    [HttpPost]
    public async Task<IActionResult> Create(CreateIngresoRequest req, CancellationToken ct)
    {
        var dto = await _svc.RegistrarAsync(req, UsuarioActualId, ct);
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    /// <summary>Actualizar ingreso.</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateIngresoRequest req, CancellationToken ct)
    {
        var dto = await _svc.ActualizarAsync(id, req, UsuarioActualId, ct);
        return Ok(dto);
    }

    /// <summary>Eliminar ingreso (soft delete).</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _svc.EliminarAsync(id, UsuarioActualId, ct);
        return NoContent();
    }

    /// <summary>Resumen mensual de ingresos.</summary>
    [HttpGet("resumen-mensual")]
    public async Task<IActionResult> ResumenMensual(
        [FromQuery] Guid hogarId, [FromQuery] int anio, [FromQuery] int mes, CancellationToken ct)
    {
        var dto = await _svc.ObtenerResumenMensualAsync(hogarId, anio, mes, ct);
        return Ok(dto);
    }

    /// <summary>Ingresos recurrentes del hogar.</summary>
    [HttpGet("recurrentes")]
    public async Task<IActionResult> Recurrentes([FromQuery] Guid hogarId, CancellationToken ct)
    {
        var lista = await _svc.ObtenerRecurrentesAsync(hogarId, ct);
        return Ok(lista);
    }
}
