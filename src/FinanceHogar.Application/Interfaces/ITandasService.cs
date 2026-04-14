using FinanceHogar.Application.DTOs.Tandas;

namespace FinanceHogar.Application.Interfaces;

public interface ITandasService
{
    Task<IReadOnlyList<TandaDto>> ObtenerPorHogarAsync(Guid hogarId, CancellationToken ct = default);
    Task<TandaDto> ObtenerPorIdAsync(Guid id, CancellationToken ct = default);
    Task<TandaDto> CrearAsync(CreateTandaRequest request, Guid organizadorId, CancellationToken ct = default);
    Task<TandaDto> ActualizarAsync(Guid id, UpdateTandaRequest request, CancellationToken ct = default);
    Task EliminarAsync(Guid id, CancellationToken ct = default);
    Task<TandaDto> AgregarParticipanteAsync(Guid tandaId, AgregarParticipanteRequest request, CancellationToken ct = default);
    Task RemoverParticipanteAsync(Guid tandaId, Guid usuarioId, CancellationToken ct = default);
    Task<TandaDto> RegistrarPagoAsync(Guid tandaId, Guid participanteId, CancellationToken ct = default);
    Task<TandaDto> AvanzarTurnoAsync(Guid tandaId, CancellationToken ct = default);
}
