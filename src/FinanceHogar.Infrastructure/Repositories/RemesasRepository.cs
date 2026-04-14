using FinanceHogar.Domain.Entities;
using FinanceHogar.Infrastructure.Data;
using FinanceHogar.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinanceHogar.Infrastructure.Repositories;

public class RemesasRepository : IRemesasRepository
{
    private readonly AppDbContext _db;
    public RemesasRepository(AppDbContext db) => _db = db;

    public Task<Remesa?> ObtenerPorIdAsync(Guid id, CancellationToken ct)
        => _db.Remesas.Include(r => r.Receptor).FirstOrDefaultAsync(r => r.Id == id, ct);

    public async Task<IReadOnlyList<Remesa>> ObtenerPorHogarAsync(
        Guid hogarId, int? anio, int? mes, CancellationToken ct)
    {
        var query = _db.Remesas.Include(r => r.Receptor).Where(r => r.HogarId == hogarId);
        if (anio.HasValue) query = query.Where(r => r.FechaRecibida.Year == anio.Value);
        if (mes.HasValue)  query = query.Where(r => r.FechaRecibida.Month == mes.Value);
        return await query.OrderByDescending(r => r.FechaRecibida).ToListAsync(ct);
    }

    public Task<decimal> ObtenerTotalAnualAsync(Guid hogarId, int anio, CancellationToken ct)
        => _db.Remesas.Where(r => r.HogarId == hogarId && r.FechaRecibida.Year == anio)
                      .SumAsync(r => r.Monto, ct);

    public async Task<Remesa> AgregarAsync(Remesa remesa, CancellationToken ct)
    {
        await _db.Remesas.AddAsync(remesa, ct);
        await _db.SaveChangesAsync(ct);
        return remesa;
    }

    public async Task<Remesa> ActualizarAsync(Remesa remesa, CancellationToken ct)
    {
        _db.Remesas.Update(remesa);
        await _db.SaveChangesAsync(ct);
        return remesa;
    }

    public async Task EliminarAsync(Guid id, CancellationToken ct)
    {
        var r = await _db.Remesas.FindAsync([id], ct);
        if (r is not null) { _db.Remesas.Remove(r); await _db.SaveChangesAsync(ct); }
    }

    public Task<bool> ExisteAsync(Guid id, CancellationToken ct)
        => _db.Remesas.AnyAsync(r => r.Id == id, ct);
}
