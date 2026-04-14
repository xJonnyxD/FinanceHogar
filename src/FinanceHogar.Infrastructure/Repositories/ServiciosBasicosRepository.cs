using FinanceHogar.Domain.Entities;
using FinanceHogar.Infrastructure.Data;
using FinanceHogar.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinanceHogar.Infrastructure.Repositories;

public class ServiciosBasicosRepository : IServiciosBasicosRepository
{
    private readonly AppDbContext _db;
    public ServiciosBasicosRepository(AppDbContext db) => _db = db;

    public Task<ServicioBasico?> ObtenerPorIdAsync(Guid id, CancellationToken ct)
        => _db.ServiciosBasicos.FirstOrDefaultAsync(s => s.Id == id, ct);

    public async Task<IReadOnlyList<ServicioBasico>> ObtenerPorHogarAsync(Guid hogarId, CancellationToken ct)
        => await _db.ServiciosBasicos.Where(s => s.HogarId == hogarId)
                    .OrderBy(s => s.FechaVencimiento).ToListAsync(ct);

    public async Task<IReadOnlyList<ServicioBasico>> ObtenerProximosVencimientosAsync(
        Guid hogarId, int dias, CancellationToken ct)
    {
        var limite = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(dias));
        var hoy = DateOnly.FromDateTime(DateTime.UtcNow);
        return await _db.ServiciosBasicos
            .Where(s => s.HogarId == hogarId
                     && s.FechaVencimiento >= hoy
                     && s.FechaVencimiento <= limite)
            .OrderBy(s => s.FechaVencimiento)
            .ToListAsync(ct);
    }

    public async Task<ServicioBasico> AgregarAsync(ServicioBasico servicio, CancellationToken ct)
    {
        await _db.ServiciosBasicos.AddAsync(servicio, ct);
        await _db.SaveChangesAsync(ct);
        return servicio;
    }

    public async Task<ServicioBasico> ActualizarAsync(ServicioBasico servicio, CancellationToken ct)
    {
        _db.ServiciosBasicos.Update(servicio);
        await _db.SaveChangesAsync(ct);
        return servicio;
    }

    public async Task EliminarAsync(Guid id, CancellationToken ct)
    {
        var s = await _db.ServiciosBasicos.FindAsync([id], ct);
        if (s is not null) { _db.ServiciosBasicos.Remove(s); await _db.SaveChangesAsync(ct); }
    }

    public Task<bool> ExisteAsync(Guid id, CancellationToken ct)
        => _db.ServiciosBasicos.AnyAsync(s => s.Id == id, ct);
}
