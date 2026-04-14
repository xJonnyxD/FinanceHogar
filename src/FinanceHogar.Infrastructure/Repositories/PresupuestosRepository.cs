using FinanceHogar.Domain.Entities;
using FinanceHogar.Infrastructure.Data;
using FinanceHogar.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinanceHogar.Infrastructure.Repositories;

public class PresupuestosRepository : IPresupuestosRepository
{
    private readonly AppDbContext _db;
    public PresupuestosRepository(AppDbContext db) => _db = db;

    public Task<PresupuestoMensual?> ObtenerPorIdAsync(Guid id, CancellationToken ct)
        => _db.PresupuestosMensuales.Include(p => p.Categoria).FirstOrDefaultAsync(p => p.Id == id, ct);

    public Task<PresupuestoMensual?> ObtenerPorHogarCategoriaYMesAsync(
        Guid hogarId, Guid categoriaId, int anio, int mes, CancellationToken ct)
        => _db.PresupuestosMensuales
            .FirstOrDefaultAsync(p => p.HogarId == hogarId
                                   && p.CategoriaId == categoriaId
                                   && p.Anio == anio
                                   && p.Mes == mes, ct);

    public async Task<IReadOnlyList<PresupuestoMensual>> ObtenerPorHogarYMesAsync(
        Guid hogarId, int anio, int mes, CancellationToken ct)
        => await _db.PresupuestosMensuales
            .Include(p => p.Categoria)
            .Where(p => p.HogarId == hogarId && p.Anio == anio && p.Mes == mes)
            .ToListAsync(ct);

    public async Task<PresupuestoMensual> AgregarAsync(PresupuestoMensual presupuesto, CancellationToken ct)
    {
        await _db.PresupuestosMensuales.AddAsync(presupuesto, ct);
        await _db.SaveChangesAsync(ct);
        return presupuesto;
    }

    public async Task<PresupuestoMensual> ActualizarAsync(PresupuestoMensual presupuesto, CancellationToken ct)
    {
        _db.PresupuestosMensuales.Update(presupuesto);
        await _db.SaveChangesAsync(ct);
        return presupuesto;
    }

    public async Task EliminarAsync(Guid id, CancellationToken ct)
    {
        var p = await _db.PresupuestosMensuales.FindAsync([id], ct);
        if (p is not null) { _db.PresupuestosMensuales.Remove(p); await _db.SaveChangesAsync(ct); }
    }

    public Task<bool> ExisteAsync(Guid hogarId, Guid categoriaId, int anio, int mes, CancellationToken ct)
        => _db.PresupuestosMensuales.AnyAsync(
            p => p.HogarId == hogarId && p.CategoriaId == categoriaId && p.Anio == anio && p.Mes == mes, ct);
}
