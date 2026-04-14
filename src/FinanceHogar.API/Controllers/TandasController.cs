using FinanceHogar.Application.DTOs.Tandas;
using FinanceHogar.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHogar.API.Controllers;

/// <summary>
/// Tandas — sistema de ahorro rotativo muy común en El Salvador.
/// Cada participante paga una cuota mensual y recibe el total en su turno asignado.
/// </summary>
[Authorize]
public class TandasController : BaseController
{
    private readonly ITandasService _svc;
    public TandasController(ITandasService svc) => _svc = svc;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid hogarId, CancellationToken ct)
    {
        var lista = await _svc.ObtenerPorHogarAsync(hogarId, ct);
        return Ok(lista);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var dto = await _svc.ObtenerPorIdAsync(id, ct);
        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTandaRequest req, CancellationToken ct)
    {
        var dto = await _svc.CrearAsync(req, UsuarioActualId, ct);
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateTandaRequest req, CancellationToken ct)
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

    /// <summary>Agregar participante a la tanda con número de turno.</summary>
    [HttpPost("{id:guid}/participantes")]
    public async Task<IActionResult> AgregarParticipante(
        Guid id, AgregarParticipanteRequest req, CancellationToken ct)
    {
        var dto = await _svc.AgregarParticipanteAsync(id, req, ct);
        return Ok(dto);
    }

    /// <summary>Remover participante de la tanda.</summary>
    [HttpDelete("{id:guid}/participantes/{usuarioId:guid}")]
    public async Task<IActionResult> RemoverParticipante(Guid id, Guid usuarioId, CancellationToken ct)
    {
        await _svc.RemoverParticipanteAsync(id, usuarioId, ct);
        return NoContent();
    }

    /// <summary>Registrar cuota pagada por un participante.</summary>
    [HttpPost("{id:guid}/registrar-pago/{participanteId:guid}")]
    public async Task<IActionResult> RegistrarPago(Guid id, Guid participanteId, CancellationToken ct)
    {
        var dto = await _svc.RegistrarPagoAsync(id, participanteId, ct);
        return Ok(dto);
    }

    /// <summary>Avanzar al siguiente turno (solo administrador del hogar).</summary>
    [HttpPost("{id:guid}/avanzar-turno")]
    public async Task<IActionResult> AvanzarTurno(Guid id, CancellationToken ct)
    {
        var dto = await _svc.AvanzarTurnoAsync(id, ct);
        return Ok(dto);
    }
}
