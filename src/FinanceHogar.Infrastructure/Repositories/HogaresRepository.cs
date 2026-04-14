using FinanceHogar.Domain.Entities;
using FinanceHogar.Infrastructure.Data;
using FinanceHogar.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinanceHogar.Infrastructure.Repositories;

public class HogaresRepository : IHogaresRepository
{
    private readonly AppDbContext _db;
    public HogaresRepository(AppDbContext db) => _db = db;

    public Task<Hogar?> ObtenerPorIdAsync(Guid id, CancellationToken ct)
        => _db.Hogares.Include(h => h.HogarUsuarios).ThenInclude(hu => hu.Usuario)
                      .Include(h => h.HogarUsuarios).ThenInclude(hu => hu.Rol)
                      .FirstOrDefaultAsync(h => h.Id == id, ct);

    public async Task<IReadOnlyList<Hogar>> ObtenerPorUsuarioAsync(Guid usuarioId, CancellationToken ct)
        => await _db.Hogares
            .Where(h => h.HogarUsuarios.Any(hu => hu.UsuarioId == usuarioId))
            .ToListAsync(ct);

    public async Task<Hogar> AgregarAsync(Hogar hogar, CancellationToken ct)
    {
        await _db.Hogares.AddAsync(hogar, ct);
        await _db.SaveChangesAsync(ct);
        return hogar;
    }

    public async Task<Hogar> ActualizarAsync(Hogar hogar, CancellationToken ct)
    {
        _db.Hogares.Update(hogar);
        await _db.SaveChangesAsync(ct);
        return hogar;
    }

    public async Task EliminarAsync(Guid id, CancellationToken ct)
    {
        var h = await _db.Hogares.FindAsync([id], ct);
        if (h is not null) { _db.Hogares.Remove(h); await _db.SaveChangesAsync(ct); }
    }

    public Task<bool> ExisteAsync(Guid id, CancellationToken ct)
        => _db.Hogares.AnyAsync(h => h.Id == id, ct);

    public Task<bool> UsuarioPerteneceAlHogarAsync(Guid hogarId, Guid usuarioId, CancellationToken ct)
        => _db.HogarUsuarios.AnyAsync(hu => hu.HogarId == hogarId && hu.UsuarioId == usuarioId, ct);

    public Task<bool> UsuarioEsAdminAsync(Guid hogarId, Guid usuarioId, CancellationToken ct)
        => _db.HogarUsuarios.AnyAsync(hu => hu.HogarId == hogarId && hu.UsuarioId == usuarioId && hu.EsAdministrador, ct);
}
