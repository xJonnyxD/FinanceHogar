using FinanceHogar.Domain.Entities;
using FinanceHogar.Infrastructure.Data;
using FinanceHogar.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinanceHogar.Infrastructure.Repositories;

public class GastosRepository : IGastosRepository
{
    private readonly AppDbContext _db;
    public GastosRepository(AppDbContext db) => _db = db;

    public async Task<Gasto?> ObtenerPorIdAsync(Guid id, CancellationToken ct)
        => await _db.Gastos
            .Include(g => g.Categoria)
            .Include(g => g.Usuario)
            .Include(g => g.ServicioBasico)
            .FirstOrDefaultAsync(g => g.Id == id, ct);

    public async Task<IReadOnlyList<Gasto>> ObtenerPorHogarAsync(
        Guid hogarId, DateOnly? desde, DateOnly? hasta, Guid? categoriaId, CancellationToken ct)
    {
        var query = _db.Gastos
            .Include(g => g.Categoria)
            .Include(g => g.Usuario)
            .Where(g => g.HogarId == hogarId);

        if (desde.HasValue) query = query.Where(g => g.FechaGasto >= desde.Value);
        if (hasta.HasValue) query = query.Where(g => g.FechaGasto <= hasta.Value);
        if (categoriaId.HasValue) query = query.Where(g => g.CategoriaId == categoriaId.Value);

        return await query.OrderByDescending(g => g.FechaGasto).ToListAsync(ct);
    }

    public async Task<IReadOnlyList<Gasto>> ObtenerRecurrentesPorHogarAsync(Guid hogarId, CancellationToken ct)
        => await _db.Gastos
            .Include(g => g.Categoria)
            .Where(g => g.HogarId == hogarId && g.EsRecurrente)
            .ToListAsync(ct);

    public async Task<decimal> ObtenerTotalMensualAsync(Guid hogarId, int anio, int mes, CancellationToken ct)
        => await _db.Gastos
            .Where(g => g.HogarId == hogarId
                     && g.FechaGasto.Year == anio
                     && g.FechaGasto.Month == mes)
            .SumAsync(g => g.MontoEnUSD ?? g.Monto, ct);

    public async Task<Dictionary<Guid, decimal>> ObtenerTotalesPorCategoriaAsync(
        Guid hogarId, int anio, int mes, CancellationToken ct)
        => await _db.Gastos
            .Where(g => g.HogarId == hogarId
                     && g.FechaGasto.Year == anio
                     && g.FechaGasto.Month == mes)
            .GroupBy(g => g.CategoriaId)
            .Select(gr => new { CategoriaId = gr.Key, Total = gr.Sum(g => g.MontoEnUSD ?? g.Monto) })
            .ToDictionaryAsync(x => x.CategoriaId, x => x.Total, ct);

    public async Task<List<(int Anio, int Mes, decimal Total)>> ObtenerTendenciasAsync(
        Guid hogarId, int meses, CancellationToken ct)
    {
        var desde = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(-meses + 1));
        var resultado = await _db.Gastos
            .Where(g => g.HogarId == hogarId && g.FechaGasto >= desde)
            .GroupBy(g => new { g.FechaGasto.Year, g.FechaGasto.Month })
            .Select(gr => new
            {
                gr.Key.Year, gr.Key.Month,
                Total = gr.Sum(g => g.MontoEnUSD ?? g.Monto)
            })
            .OrderBy(x => x.Year).ThenBy(x => x.Month)
            .ToListAsync(ct);

        return resultado.Select(x => (x.Year, x.Month, x.Total)).ToList();
    }

    public async Task<Gasto> AgregarAsync(Gasto gasto, CancellationToken ct)
    {
        await _db.Gastos.AddAsync(gasto, ct);
        await _db.SaveChangesAsync(ct);
        return gasto;
    }

    public async Task<Gasto> ActualizarAsync(Gasto gasto, CancellationToken ct)
    {
        _db.Gastos.Update(gasto);
        await _db.SaveChangesAsync(ct);
        return gasto;
    }

    public async Task EliminarAsync(Guid id, CancellationToken ct)
    {
        var gasto = await _db.Gastos.FindAsync([id], ct);
        if (gasto is not null)
        {
            _db.Gastos.Remove(gasto);
            await _db.SaveChangesAsync(ct);
        }
    }

    public Task<bool> ExisteAsync(Guid id, CancellationToken ct)
        => _db.Gastos.AnyAsync(g => g.Id == id, ct);

    public Task<bool> PerteneceAlHogarAsync(Guid gastoId, Guid hogarId, CancellationToken ct)
        => _db.Gastos.AnyAsync(g => g.Id == gastoId && g.HogarId == hogarId, ct);
}
