using FinanceHogar.Application.DTOs.Hogares;
using FinanceHogar.Application.Interfaces;
using FinanceHogar.Application.Interfaces.Repositories;
using FinanceHogar.Domain.Entities;
using FinanceHogar.Domain.Enums;

namespace FinanceHogar.Application.Services;

public class HogaresService : IHogaresService
{
    private readonly IHogaresRepository  _hogares;
    private readonly IUsuariosRepository _usuarios;

    public HogaresService(IHogaresRepository hogares, IUsuariosRepository usuarios)
    {
        _hogares  = hogares;
        _usuarios = usuarios;
    }

    public async Task<IReadOnlyList<HogarDto>> ObtenerPorUsuarioAsync(Guid usuarioId, CancellationToken ct)
    {
        var lista = await _hogares.ObtenerPorUsuarioAsync(usuarioId, ct);
        return lista.Select(Mapear).ToList();
    }

    public async Task<HogarDto> ObtenerPorIdAsync(Guid id, CancellationToken ct)
    {
        var hogar = await _hogares.ObtenerPorIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Hogar {id} no encontrado.");
        return Mapear(hogar);
    }

    public async Task<HogarDto> CrearAsync(CreateHogarRequest req, Guid usuarioId, CancellationToken ct)
    {
        var hogar = new Hogar
        {
            Nombre                 = req.Nombre,
            Departamento           = req.Departamento,
            Municipio              = req.Municipio,
            MonedaPrincipal        = Enum.TryParse<TipoMoneda>(req.MonedaPrincipal, out var m) ? m : TipoMoneda.USD,
            PresupuestoMensualTotal = req.PresupuestoMensualTotal
        };
        hogar = await _hogares.AgregarAsync(hogar, ct);

        // Agregar al creador como administrador
        var usuario = await _usuarios.ObtenerPorIdAsync(usuarioId, ct);
        if (usuario is not null)
        {
            usuario.HogarUsuarios ??= [];
            usuario.HogarUsuarios.Add(new HogarUsuario
            {
                HogarId         = hogar.Id,
                UsuarioId       = usuarioId,
                EsAdministrador = true
            });
            await _usuarios.ActualizarAsync(usuario, ct);
        }
        return Mapear(hogar);
    }

    public async Task<HogarDto> ActualizarAsync(Guid id, UpdateHogarRequest req, Guid usuarioId, CancellationToken ct)
    {
        var hogar = await _hogares.ObtenerPorIdAsync(id, ct)
            ?? throw new KeyNotFoundException($"Hogar {id} no encontrado.");

        if (!await _hogares.UsuarioEsAdminAsync(id, usuarioId, ct))
            throw new UnauthorizedAccessException("Solo administradores pueden editar el hogar.");

        hogar.Nombre                  = req.Nombre;
        hogar.Departamento            = req.Departamento;
        hogar.Municipio               = req.Municipio;
        hogar.PresupuestoMensualTotal  = req.PresupuestoMensualTotal;
        var actualizado = await _hogares.ActualizarAsync(hogar, ct);
        return Mapear(actualizado);
    }

    public async Task EliminarAsync(Guid id, Guid usuarioId, CancellationToken ct)
    {
        if (!await _hogares.ExisteAsync(id, ct))
            throw new KeyNotFoundException($"Hogar {id} no encontrado.");
        if (!await _hogares.UsuarioEsAdminAsync(id, usuarioId, ct))
            throw new UnauthorizedAccessException("Solo administradores pueden eliminar el hogar.");
        await _hogares.EliminarAsync(id, ct);
    }

    public async Task InvitarMiembroAsync(Guid hogarId, InvitarMiembroRequest req, Guid adminId, CancellationToken ct)
    {
        if (!await _hogares.UsuarioEsAdminAsync(hogarId, adminId, ct))
            throw new UnauthorizedAccessException("Solo administradores pueden invitar miembros.");

        var usuario = await _usuarios.ObtenerPorEmailAsync(req.Email, ct)
            ?? throw new KeyNotFoundException($"No existe usuario con email '{req.Email}'.");

        if (await _hogares.UsuarioPerteneceAlHogarAsync(hogarId, usuario.Id, ct))
            throw new InvalidOperationException("El usuario ya pertenece a este hogar.");

        usuario.HogarUsuarios ??= [];
        usuario.HogarUsuarios.Add(new HogarUsuario
        {
            HogarId         = hogarId,
            UsuarioId       = usuario.Id,
            EsAdministrador = req.EsAdministrador
        });
        await _usuarios.ActualizarAsync(usuario, ct);
    }

    public async Task RemoverMiembroAsync(Guid hogarId, Guid miembroId, Guid adminId, CancellationToken ct)
    {
        if (!await _hogares.UsuarioEsAdminAsync(hogarId, adminId, ct))
            throw new UnauthorizedAccessException("Solo administradores pueden remover miembros.");

        var usuario = await _usuarios.ObtenerPorIdAsync(miembroId, ct)
            ?? throw new KeyNotFoundException("Usuario no encontrado.");

        var rel = usuario.HogarUsuarios?.FirstOrDefault(hu => hu.HogarId == hogarId)
            ?? throw new KeyNotFoundException("El usuario no pertenece a este hogar.");

        usuario.HogarUsuarios!.Remove(rel);
        await _usuarios.ActualizarAsync(usuario, ct);
    }

    private static HogarDto Mapear(Hogar h) => new()
    {
        Id                     = h.Id,
        Nombre                 = h.Nombre,
        Pais                   = h.Pais,
        Departamento           = h.Departamento,
        Municipio              = h.Municipio,
        MonedaPrincipal        = h.MonedaPrincipal.ToString(),
        PresupuestoMensualTotal = h.PresupuestoMensualTotal,
        CreatedAt              = h.CreatedAt
    };
}
