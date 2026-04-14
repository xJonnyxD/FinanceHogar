using FinanceHogar.Application.DTOs.Hogares;

namespace FinanceHogar.Application.Interfaces;

public interface IHogaresService
{
    Task<IReadOnlyList<HogarDto>> ObtenerPorUsuarioAsync(Guid usuarioId, CancellationToken ct = default);
    Task<HogarDto> ObtenerPorIdAsync(Guid id, CancellationToken ct = default);
    Task<HogarDto> CrearAsync(CreateHogarRequest request, Guid usuarioId, CancellationToken ct = default);
    Task<HogarDto> ActualizarAsync(Guid id, UpdateHogarRequest request, Guid usuarioId, CancellationToken ct = default);
    Task EliminarAsync(Guid id, Guid usuarioId, CancellationToken ct = default);
    Task InvitarMiembroAsync(Guid hogarId, InvitarMiembroRequest request, Guid adminId, CancellationToken ct = default);
    Task RemoverMiembroAsync(Guid hogarId, Guid miembroId, Guid adminId, CancellationToken ct = default);
}
