using FinanceHogar.Application.DTOs.ServiciosBasicos;

namespace FinanceHogar.Application.Interfaces;

public interface IServiciosBasicosService
{
    Task<IReadOnlyList<ServicioBasicoDto>> ObtenerPorHogarAsync(Guid hogarId, CancellationToken ct = default);
    Task<ServicioBasicoDto> ObtenerPorIdAsync(Guid id, CancellationToken ct = default);
    Task<ServicioBasicoDto> CrearAsync(CreateServicioBasicoRequest request, Guid usuarioId, CancellationToken ct = default);
    Task<ServicioBasicoDto> ActualizarAsync(Guid id, UpdateServicioBasicoRequest request, CancellationToken ct = default);
    Task EliminarAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<ServicioBasicoDto>> ObtenerVencimientosProximosAsync(Guid hogarId, int dias, CancellationToken ct = default);
    Task<ServicioBasicoDto> PagarAsync(Guid id, Guid usuarioId, CancellationToken ct = default);
}
