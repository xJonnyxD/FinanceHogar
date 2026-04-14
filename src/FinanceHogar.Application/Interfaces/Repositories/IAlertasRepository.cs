using FinanceHogar.Domain.Entities;
using FinanceHogar.Domain.Enums;

namespace FinanceHogar.Application.Interfaces.Repositories;

public interface IAlertasRepository
{
    Task<Alerta?> ObtenerPorIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Alerta>> ObtenerPorHogarAsync(Guid hogarId, EstadoAlerta? estado, CancellationToken ct = default);
    Task<int> ContarNoLeidasAsync(Guid hogarId, CancellationToken ct = default);
    Task<bool> ExisteAlertaTemporadaEnMesAsync(Guid hogarId, TipoAlerta tipo, int anio, int mes, CancellationToken ct = default);
    Task<Alerta> AgregarAsync(Alerta alerta, CancellationToken ct = default);
    Task<Alerta> ActualizarAsync(Alerta alerta, CancellationToken ct = default);
    Task EliminarAsync(Guid id, CancellationToken ct = default);
}
