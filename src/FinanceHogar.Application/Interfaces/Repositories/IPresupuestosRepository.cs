using FinanceHogar.Domain.Entities;

namespace FinanceHogar.Application.Interfaces.Repositories;

public interface IPresupuestosRepository
{
    Task<PresupuestoMensual?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default);
    Task<PresupuestoMensual?> ObtenerPorHogarCategoriaYMesAsync(Guid hogarId, Guid categoriaId, int anio, int mes, CancellationToken ct = default);
    Task<IReadOnlyList<PresupuestoMensual>> ObtenerPorHogarYMesAsync(Guid hogarId, int anio, int mes, CancellationToken ct = default);
    Task<PresupuestoMensual> AgregarAsync(PresupuestoMensual presupuesto, CancellationToken ct = default);
    Task<PresupuestoMensual> ActualizarAsync(PresupuestoMensual presupuesto, CancellationToken ct = default);
    Task EliminarAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExisteAsync(Guid hogarId, Guid categoriaId, int anio, int mes, CancellationToken ct = default);
}
