using FinanceHogar.Domain.Entities;
using FinanceHogar.Domain.Enums;
using FinanceHogar.Infrastructure.Data;
using FinanceHogar.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinanceHogar.Infrastructure.Repositories;

public class AlertasRepository : IAlertasRepository
{
    private readonly AppDbContext _db;
    public AlertasRepository(AppDbContext db) => _db = db;

    public Task<Alerta?> ObtenerPorIdAsync(Guid id, CancellationToken ct)
        => _db.Alertas.FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task<IReadOnlyList<Alerta>> ObtenerPorHogarAsync(
        Guid hogarId, EstadoAlerta? estado, CancellationToken ct)
    {
        var query = _db.Alertas
            .Include(a => a.Categoria)
            .Where(a => a.HogarId == hogarId);

        if (estado.HasValue) query = query.Where(a => a.Estado == estado.Value);

        return await query.OrderByDescending(a => a.FechaGenerada).ToListAsync(ct);
    }

    public Task<int> ContarNoLeidasAsync(Guid hogarId, CancellationToken ct)
        => _db.Alertas.CountAsync(a => a.HogarId == hogarId && a.Estado == EstadoAlerta.Pendiente, ct);

    public Task<bool> ExisteAlertaTemporadaEnMesAsync(
        Guid hogarId, TipoAlerta tipo, int anio, int mes, CancellationToken ct)
        => _db.Alertas.AnyAsync(a => a.HogarId == hogarId
                                  && a.Tipo == tipo
                                  && a.FechaGenerada.Year == anio
                                  && a.FechaGenerada.Month == mes, ct);

    public async Task<Alerta> AgregarAsync(Alerta alerta, CancellationToken ct)
    {
        await _db.Alertas.AddAsync(alerta, ct);
        await _db.SaveChangesAsync(ct);
        return alerta;
    }

    public async Task<Alerta> ActualizarAsync(Alerta alerta, CancellationToken ct)
    {
        _db.Alertas.Update(alerta);
        await _db.SaveChangesAsync(ct);
        return alerta;
    }

    public async Task EliminarAsync(Guid id, CancellationToken ct)
    {
        var a = await _db.Alertas.FindAsync([id], ct);
        if (a is not null) { _db.Alertas.Remove(a); await _db.SaveChangesAsync(ct); }
    }
}
