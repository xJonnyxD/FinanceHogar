using FinanceHogar.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHogar.API.Controllers;

[Authorize]
public class ReportesController : BaseController
{
    private readonly IReportesService _svc;
    public ReportesController(IReportesService svc) => _svc = svc;

    /// <summary>Balance mensual: ingresos, gastos y desglose por categoría.</summary>
    [HttpGet("balance-mensual")]
    public async Task<IActionResult> BalanceMensual(
        [FromQuery] Guid hogarId, [FromQuery] int anio, [FromQuery] int mes, CancellationToken ct)
    {
        var dto = await _svc.ObtenerBalanceMensualAsync(hogarId, anio, mes, ct);
        return Ok(dto);
    }

    /// <summary>Tendencias de ingresos y gastos en los últimos N meses.</summary>
    [HttpGet("tendencias")]
    public async Task<IActionResult> Tendencias(
        [FromQuery] Guid hogarId, [FromQuery] int meses = 6, CancellationToken ct = default)
    {
        var lista = await _svc.ObtenerTendenciasAsync(hogarId, meses, ct);
        var resultado = lista.Select(t => new
        {
            anio     = t.Anio,
            mes      = t.Mes,
            ingresos = t.Ingresos,
            gastos   = t.Gastos,
            balance  = t.Balance
        });
        return Ok(resultado);
    }

    /// <summary>Puntaje financiero familiar (0-100).</summary>
    [HttpGet("puntaje-financiero")]
    public async Task<IActionResult> PuntajeFinanciero(
        [FromQuery] Guid hogarId, CancellationToken ct)
    {
        var dto = await _svc.ObtenerPuntajeFinancieroAsync(hogarId, ct);
        return Ok(dto);
    }
}
