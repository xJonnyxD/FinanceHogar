using FinanceHogar.Application.DTOs.Gastos;

namespace FinanceHogar.Application.Interfaces;

public interface IGastosService
{
    Task<IReadOnlyList<GastoDto>> ObtenerPorHogarAsync(Guid hogarId, DateOnly? desde, DateOnly? hasta, Guid? categoriaId, CancellationToken ct = default);
    Task<GastoDto> ObtenerPorIdAsync(Guid id, CancellationToken ct = default);
    Task<GastoDto> RegistrarAsync(CreateGastoRequest request, Guid usuarioId, CancellationToken ct = default);
    Task<GastoDto> ActualizarAsync(Guid id, UpdateGastoRequest request, Guid usuarioId, CancellationToken ct = default);
    Task EliminarAsync(Guid id, Guid usuarioId, CancellationToken ct = default);
    Task<IReadOnlyList<GastoDto>> ObtenerRecurrentesAsync(Guid hogarId, CancellationToken ct = default);
}
