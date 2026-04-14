using FinanceHogar.Domain.Entities;

namespace FinanceHogar.Application.Interfaces.Repositories;

public interface IServiciosBasicosRepository
{
    Task<ServicioBasico?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<ServicioBasico>> ObtenerPorHogarAsync(Guid hogarId, CancellationToken ct = default);
    Task<IReadOnlyList<ServicioBasico>> ObtenerProximosVencimientosAsync(Guid hogarId, int dias, CancellationToken ct = default);
    Task<ServicioBasico> AgregarAsync(ServicioBasico servicio, CancellationToken ct = default);
    Task<ServicioBasico> ActualizarAsync(ServicioBasico servicio, CancellationToken ct = default);
    Task EliminarAsync(Guid id, CancellationToken ct = default);
    Task<bool> ExisteAsync(Guid id, CancellationToken ct = default);
}
