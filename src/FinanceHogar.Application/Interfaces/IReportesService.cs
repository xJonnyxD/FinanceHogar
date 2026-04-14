using FinanceHogar.Application.DTOs.Reportes;

namespace FinanceHogar.Application.Interfaces;

public interface IReportesService
{
    Task<BalanceMensualDto> ObtenerBalanceMensualAsync(Guid hogarId, int anio, int mes, CancellationToken ct = default);
    Task<PuntajeFinancieroDto> ObtenerPuntajeFinancieroAsync(Guid hogarId, CancellationToken ct = default);
    Task<List<(int Anio, int Mes, decimal Ingresos, decimal Gastos, decimal Balance)>> ObtenerTendenciasAsync(Guid hogarId, int meses, CancellationToken ct = default);
}
