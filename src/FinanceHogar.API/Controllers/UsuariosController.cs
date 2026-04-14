using FinanceHogar.Application.DTOs.Usuarios;
using FinanceHogar.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHogar.API.Controllers;

[Authorize]
public class UsuariosController : BaseController
{
    private readonly IUsuariosRepository _usuarios;

    public UsuariosController(IUsuariosRepository usuarios) => _usuarios = usuarios;

    /// <summary>Obtener perfil del usuario autenticado.</summary>
    [HttpGet("perfil")]
    public async Task<IActionResult> GetPerfil(CancellationToken ct)
    {
        var usuario = await _usuarios.ObtenerPorIdAsync(UsuarioActualId, ct);
        if (usuario is null) return NotFound();
        return Ok(new UsuarioDto
        {
            Id             = usuario.Id,
            NombreCompleto = usuario.NombreCompleto,
            Email          = usuario.Email,
            Telefono       = usuario.Telefono,
            DUI            = usuario.DUI,
            CreatedAt      = usuario.CreatedAt
        });
    }

    /// <summary>Actualizar perfil del usuario autenticado.</summary>
    [HttpPut("perfil")]
    public async Task<IActionResult> UpdatePerfil(UpdateUsuarioRequest req, CancellationToken ct)
    {
        var usuario = await _usuarios.ObtenerPorIdAsync(UsuarioActualId, ct);
        if (usuario is null) return NotFound();

        usuario.NombreCompleto = req.NombreCompleto;
        usuario.Telefono       = req.Telefono;
        usuario.DUI            = req.DUI;
        await _usuarios.ActualizarAsync(usuario, ct);

        return Ok(new UsuarioDto
        {
            Id             = usuario.Id,
            NombreCompleto = usuario.NombreCompleto,
            Email          = usuario.Email,
            Telefono       = usuario.Telefono,
            DUI            = usuario.DUI,
            CreatedAt      = usuario.CreatedAt
        });
    }

    /// <summary>Eliminar cuenta del usuario autenticado (soft delete).</summary>
    [HttpDelete("perfil")]
    public async Task<IActionResult> DeletePerfil(CancellationToken ct)
    {
        var usuario = await _usuarios.ObtenerPorIdAsync(UsuarioActualId, ct);
        if (usuario is null) return NotFound();
        usuario.IsDeleted  = true;
        usuario.DeletedAt  = DateTime.UtcNow;
        await _usuarios.ActualizarAsync(usuario, ct);
        return NoContent();
    }
}
