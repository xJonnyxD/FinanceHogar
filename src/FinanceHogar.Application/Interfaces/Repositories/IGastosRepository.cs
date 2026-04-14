using FinanceHogar.Domain.Entities;

namespace FinanceHogar.Application.Interfaces.Repositories;

public interface IGastosRepository
{
    Task<Gasto?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Gasto>> ObtenerPorHogarAsync(Guid hogarId, DateOnly? desde, DateOnly? hasta, Guid? categoriaId, CancellationToken ct = default);
    Task<IReadOnlyList<Gasto>> ObtenerRecurrentesPorHogarAsync(Guid hogarId, CancellationToken ct = default);
    Task<decimal> ObtenerTotalMensualAsync(Guid hogarId, int anio, int mes, CancellationToken ct = default);
    Task<Dictionary<Guid, decimal>> ObtenerTotalesPorCategoriaAsync(Guid hogarId, int anio, int mes, CancellationToken ct = default);
    Task<List<(int Anio, int Mes, decimal Total)>> ObtenerTendenciasAsync(Guid hogarId, int meses, CancellationToken ct = default);
    Task<Gasto> AgregarAsync(Gasto gasto, CancellationToken ct = default);
    Task<Gasto> ActualizarAsync(Gasto gasto, CancellationToken ct = default);
    Task EliminarAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExisteAsync(Guid id, CancellationToken ct = default);
    Task<bool> PerteneceAlHogarAsync(Guid gastoId, Guid hogarId, CancellationToken ct = default);
}
