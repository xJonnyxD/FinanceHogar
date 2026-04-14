using FinanceHogar.Domain.Entities;
using FinanceHogar.Infrastructure.Data;
using FinanceHogar.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinanceHogar.Infrastructure.Repositories;

public class IngresosRepository : IIngresosRepository
{
    private readonly AppDbContext _db;
    public IngresosRepository(AppDbContext db) => _db = db;

    public async Task<Ingreso?> ObtenerPorIdAsync(Guid id, CancellationToken ct)
        => await _db.Ingresos
            .Include(i => i.Categoria)
            .Include(i => i.Usuario)
            .Include(i => i.Remesa)
            .FirstOrDefaultAsync(i => i.Id == id, ct);

    public async Task<IReadOnlyList<Ingreso>> ObtenerPorHogarAsync(
        Guid hogarId, DateOnly? desde, DateOnly? hasta, Guid? categoriaId, CancellationToken ct)
    {
        var query = _db.Ingresos
            .Include(i => i.Categoria)
            .Include(i => i.Usuario)
            .Where(i => i.HogarId == hogarId);

        if (desde.HasValue) query = query.Where(i => i.FechaIngreso >= desde.Value);
        if (hasta.HasValue) query = query.Where(i => i.FechaIngreso <= hasta.Value);
        if (categoriaId.HasValue) query = query.Where(i => i.CategoriaId == categoriaId.Value);

        return await query.OrderByDescending(i => i.FechaIngreso).ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Ingreso>> ObtenerRecurrentesPorHogarAsync(Guid hogarId, CancellationToken ct)
        => await _db.Ingresos
            .Include(i => i.Categoria)
            .Where(i => i.HogarId == hogarId && i.EsRecurrente)
            .ToListAsync(ct);

    public async Task<decimal> ObtenerTotalMensualAsync(Guid hogarId, int anio, int mes, CancellationToken ct)
        => await _db.Ingresos
            .Where(i => i.HogarId == hogarId
                     && i.FechaIngreso.Year == anio
                     && i.FechaIngreso.Month == mes)
            .SumAsync(i => i.MontoEnUSD ?? i.Monto, ct);

    public async Task<Dictionary<int, decimal>> ObtenerTotalesPorTipoAsync(
        Guid hogarId, int anio, int mes, CancellationToken ct)
        => await _db.Ingresos
            .Where(i => i.HogarId == hogarId
                     && i.FechaIngreso.Year == anio
                     && i.FechaIngreso.Month == mes)
            .GroupBy(i => (int)i.Tipo)
            .Select(g => new { Tipo = g.Key, Total = g.Sum(i => i.MontoEnUSD ?? i.Monto) })
            .ToDictionaryAsync(x => x.Tipo, x => x.Total, ct);

    public async Task<List<(int Anio, int Mes, decimal Total)>> ObtenerTendenciasAsync(
        Guid hogarId, int meses, CancellationToken ct)
    {
        var desde = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-meses + 1));
        var resultado = await _db.Ingresos
            .Where(i => i.HogarId == hogarId && i.FechaIngreso >= desde)
            .GroupBy(i => new { i.FechaIngreso.Year, i.FechaIngreso.Month })
            .Select(g => new
            {
                g.Key.Year, g.Key.Month,
                Total = g.Sum(i => i.MontoEnUSD ?? i.Monto)
            })
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .ToListAsync(ct);

        return resultado.Select(x => (x.Year, x.Month, x.Total)).ToList();
    }

    public async Task<Ingreso> AgregarAsync(Ingreso ingreso, CancellationToken ct)
    {
        await _db.Ingresos.AddAsync(ingreso, ct);
        await _db.SaveChangesAsync(ct);
        return ingreso;
    }

    public async Task<Ingreso> ActualizarAsync(Ingreso ingreso, CancellationToken ct)
    {
        _db.Ingresos.Update(ingreso);
        await _db.SaveChangesAsync(ct);
        return ingreso;
    }

    public async Task EliminarAsync(Guid id, CancellationToken ct)
    {
        var ingreso = await _db.Ingresos.FindAsync([id], ct);
        if (ingreso is not null)
        {
            _db.Ingresos.Remove(ingreso);
            await _db.SaveChangesAsync(ct);
        }
    }

    public Task<bool> ExisteAsync(Guid id, CancellationToken ct)
        => _db.Ingresos.AnyAsync(i => i.Id == id, ct);

    public Task<bool> PerteneceAlHogarAsync(Guid ingresoId, Guid hogarId, CancellationToken ct)
        => _db.Ingresos.AnyAsync(i => i.Id == ingresoId && i.HogarId == hogarId, ct);
}
