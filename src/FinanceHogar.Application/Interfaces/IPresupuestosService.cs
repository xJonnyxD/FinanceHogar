using FinanceHogar.Application.DTOs.Presupuestos;

namespace FinanceHogar.Application.Interfaces;

public interface IPresupuestosService
{
    Task<IReadOnlyList<PresupuestoDto>> ObtenerPorHogarYMesAsync(Guid hogarId, int anio, int mes, CancellationToken ct = default);
    Task<PresupuestoDto> ObtenerPorIdAsync(Guid id, CancellationToken ct = default);
    Task<PresupuestoDto> CrearAsync(CreatePresupuestoRequest request, CancellationToken ct = default);
    Task<PresupuestoDto> ActualizarAsync(Guid id, UpdatePresupuestoRequest request, CancellationToken ct = default);
    Task EliminarAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<PresupuestoVsRealDto>> ObtenerVsRealAsync(Guid hogarId, int anio, int mes, CancellationToken ct = default);
    Task<IReadOnlyList<PresupuestoDto>> CopiarMesAnteriorAsync(CopiarPresupuestoRequest request, CancellationToken ct = default);
}
