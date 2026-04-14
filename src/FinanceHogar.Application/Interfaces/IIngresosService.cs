using FinanceHogar.Application.DTOs.Ingresos;

namespace FinanceHogar.Application.Interfaces;

public interface IIngresosService
{
    Task<IReadOnlyList<IngresoDto>> ObtenerPorHogarAsync(Guid hogarId, DateOnly? desde, DateOnly? hasta, Guid? categoriaId, CancellationToken ct = default);
    Task<IngresoDto> ObtenerPorIdAsync(Guid id, CancellationToken ct = default);
    Task<IngresoDto> RegistrarAsync(CreateIngresoRequest request, Guid usuarioId, CancellationToken ct = default);
    Task<IngresoDto> ActualizarAsync(Guid id, UpdateIngresoRequest request, Guid usuarioId, CancellationToken ct = default);
    Task EliminarAsync(Guid id, Guid usuarioId, CancellationToken ct = default);
    Task<ResumenMensualIngresosDto> ObtenerResumenMensualAsync(Guid hogarId, int anio, int mes, CancellationToken ct = default);
    Task<IReadOnlyList<IngresoDto>> ObtenerRecurrentesAsync(Guid hogarId, CancellationToken ct = default);
}
