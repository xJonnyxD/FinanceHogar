using FinanceHogar.Domain.Entities;
using FinanceHogar.Infrastructure.Data;
using FinanceHogar.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinanceHogar.Infrastructure.Repositories;

public class TandasRepository : ITandasRepository
{
    private readonly AppDbContext _db;
    public TandasRepository(AppDbContext db) => _db = db;

    public Task<Tanda?> ObtenerPorIdAsync(Guid id, CancellationToken ct)
        => _db.Tandas.Include(t => t.Participantes).ThenInclude(p => p.Usuario)
                     .Include(t => t.Organizador)
                     .FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task<IReadOnlyList<Tanda>> ObtenerPorHogarAsync(Guid hogarId, CancellationToken ct)
        => await _db.Tandas.Include(t => t.Participantes)
                    .Where(t => t.HogarId == hogarId)
                    .OrderByDescending(t => t.CreatedAt)
                    .ToListAsync(ct);

    public Task<TandaParticipante?> ObtenerParticipanteAsync(Guid tandaId, Guid usuarioId, CancellationToken ct)
        => _db.TandaParticipantes.FirstOrDefaultAsync(
            tp => tp.TandaId == tandaId && tp.UsuarioId == usuarioId, ct);

    public async Task<Tanda> AgregarAsync(Tanda tanda, CancellationToken ct)
    {
        await _db.Tandas.AddAsync(tanda, ct);
        await _db.SaveChangesAsync(ct);
        return tanda;
    }

    public async Task<Tanda> ActualizarAsync(Tanda tanda, CancellationToken ct)
    {
        _db.Tandas.Update(tanda);
        await _db.SaveChangesAsync(ct);
        return tanda;
    }

    public async Task EliminarAsync(Guid id, CancellationToken ct)
    {
        var t = await _db.Tandas.FindAsync([id], ct);
        if (t is not null) { _db.Tandas.Remove(t); await _db.SaveChangesAsync(ct); }
    }

    public Task<bool> ExisteAsync(Guid id, CancellationToken ct)
        => _db.Tandas.AnyAsync(t => t.Id == id, ct);
}
