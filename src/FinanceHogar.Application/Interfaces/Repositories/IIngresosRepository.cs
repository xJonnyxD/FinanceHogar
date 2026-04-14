using FinanceHogar.Domain.Entities;

namespace FinanceHogar.Application.Interfaces.Repositories;

public interface IIngresosRepository
{
    Task<Ingreso?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Ingreso>> ObtenerPorHogarAsync(Guid hogarId, DateOnly? desde, DateOnly? hasta, Guid? categoriaId, CancellationToken ct = default);
    Task<IReadOnlyList<Ingreso>> ObtenerRecurrentesPorHogarAsync(Guid hogarId, CancellationToken ct = default);
    Task<decimal> ObtenerTotalMensualAsync(Guid hogarId, int anio, int mes, CancellationToken ct = default);
    Task<Dictionary<int, decimal>> ObtenerTotalesPorTipoAsync(Guid hogarId, int anio, int mes, CancellationToken ct = default);
    Task<List<(int Anio, int Mes, decimal Total)>> ObtenerTendenciasAsync(Guid hogarId, int meses, CancellationToken ct = default);
    Task<Ingreso> AgregarAsync(Ingreso ingreso, CancellationToken ct = default);
    Task<Ingreso> ActualizarAsync(Ingreso ingreso, CancellationToken ct = default);
    Task EliminarAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExisteAsync(Guid id, CancellationToken ct = default);
    Task<bool> PerteneceAlHogarAsync(Guid ingresoId, Guid hogarId, CancellationToken ct = default);
}
