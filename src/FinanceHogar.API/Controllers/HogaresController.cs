using FinanceHogar.Application.DTOs.Hogares;
using FinanceHogar.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHogar.API.Controllers;

[Authorize]
public class HogaresController : BaseController
{
    private readonly IHogaresService _svc;
    public HogaresController(IHogaresService svc) => _svc = svc;

    /// <summary>Obtener hogares a los que pertenece el usuario actual.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var lista = await _svc.ObtenerPorUsuarioAsync(UsuarioActualId, ct);
        return Ok(lista);
    }

    /// <summary>Obtener hogar por ID.</summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var dto = await _svc.ObtenerPorIdAsync(id, ct);
        return Ok(dto);
    }

    /// <summary>Crear nuevo hogar.</summary>
    [HttpPost]
    public async Task<IActionResult> Create(CreateHogarRequest req, CancellationToken ct)
    {
        var dto = await _svc.CrearAsync(req, UsuarioActualId, ct);
        return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
    }

    /// <summary>Actualizar hogar (solo administradores).</summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateHogarRequest req, CancellationToken ct)
    {
        var dto = await _svc.ActualizarAsync(id, req, UsuarioActualId, ct);
        return Ok(dto);
    }

    /// <summary>Eliminar hogar (solo administradores).</summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _svc.EliminarAsync(id, UsuarioActualId, ct);
        return NoContent();
    }

    /// <summary>Invitar miembro al hogar por email (solo administradores).</summary>
    [HttpPost("{id:guid}/invitar")]
    public async Task<IActionResult> Invitar(Guid id, InvitarMiembroRequest req, CancellationToken ct)
    {
        await _svc.InvitarMiembroAsync(id, req, UsuarioActualId, ct);
        return NoContent();
    }

    /// <summary>Remover miembro del hogar (solo administradores).</summary>
    [HttpDelete("{id:guid}/miembros/{usuarioId:guid}")]
    public async Task<IActionResult> RemoverMiembro(Guid id, Guid usuarioId, CancellationToken ct)
    {
        await _svc.RemoverMiembroAsync(id, usuarioId, UsuarioActualId, ct);
        return NoContent();
    }
}
