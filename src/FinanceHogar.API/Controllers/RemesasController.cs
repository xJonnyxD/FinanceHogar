using FinanceHogar.Application.DTOs.Remesas;
using FinanceHogar.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHogar.API.Controllers;

/// <summary>
/// Remesas — El Salvador recibe ~$8B anuales en remesas del exterior.
/// Registrar una remesa crea automáticamente un Ingreso de tipo Remesa.
/// </summary>
[Authorize]
public class RemesasController : BaseController
{
    private readonly IRemesasService _svc;
    public RemesasController(IRemesasService svc) => _svc = svc;

    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid hogarId,
        [FromQuery] int? anio,
        [FromQuery] int? mes,
        CancellationToken ct)
    {
        var lista = await _svc.ObtenerPorHogarAsync(hogarId, anio, mes, ct);
        return Ok(lista);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var dto = await _svc.ObtenerPorIdAsync(id, ct);
        return Ok(dto);
    }

    /// <summary>
    /// Registrar remesa recibida. Crea automáticamente un Ingreso de tipo Remesa.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create(CreateRemesaRequest req, CancellationToken ct)
    {
        var dto = await _svc.RegistrarAsync(req, UsuarioActualId, ct);
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateRemesaRequest req, CancellationToken ct)
    {
        var dto = await _svc.ActualizarAsync(id, req, ct);
        return Ok(dto);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _svc.EliminarAsync(id, ct);
        return NoContent();
    }

    /// <summary>Estadísticas anuales de remesas: total, promedio mensual, empresa más frecuente.</summary>
    [HttpGet("estadisticas")]
    public async Task<IActionResult> Estadisticas(
        [FromQuery] Guid hogarId,
        [FromQuery] int? anio,
        CancellationToken ct)
    {
        var anioConsulta = anio ?? DateTime.UtcNow.Year;
        var dto = await _svc.ObtenerEstadisticasAsync(hogarId, anioConsulta, ct);
        return Ok(dto);
    }
}
