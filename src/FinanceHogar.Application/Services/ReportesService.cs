using FinanceHogar.Application.DTOs.Reportes;
using FinanceHogar.Application.Interfaces;
using FinanceHogar.Domain.BusinessRules;
using FinanceHogar.Application.Interfaces.Repositories;

namespace FinanceHogar.Application.Services;

public class ReportesService : IReportesService
{
    private readonly IIngresosRepository _ingresos;
    private readonly IGastosRepository _gastos;
    private readonly IPresupuestosRepository _presupuestos;
    private readonly IServiciosBasicosRepository _servicios;

    public ReportesService(
        IIngresosRepository ingresos,
        IGastosRepository gastos,
        IPresupuestosRepository presupuestos,
        IServiciosBasicosRepository servicios)
    {
        _ingresos = ingresos;
        _gastos = gastos;
        _presupuestos = presupuestos;
        _servicios = servicios;
    }

    public async Task<BalanceMensualDto> ObtenerBalanceMensualAsync(
        Guid hogarId, int anio, int mes, CancellationToken ct)
    {
        var totalIngresos = await _ingresos.ObtenerTotalMensualAsync(hogarId, anio, mes, ct);
        var totalGastos   = await _gastos.ObtenerTotalMensualAsync(hogarId, anio, mes, ct);

        var (anioAnt, mesAnt) = mes == 1 ? (anio - 1, 12) : (anio, mes - 1);
        var ingresosAnt = await _ingresos.ObtenerTotalMensualAsync(hogarId, anioAnt, mesAnt, ct);
        var gastosAnt   = await _gastos.ObtenerTotalMensualAsync(hogarId, anioAnt, mesAnt, ct);

        var gastosPorCat = await _gastos.ObtenerTotalesPorCategoriaAsync(hogarId, anio, mes, ct);
        var presupuestos = await _presupuestos.ObtenerPorHogarYMesAsync(hogarId, anio, mes, ct);

        var catDetalle = gastosPorCat.Select(kv =>
        {
            var cat = presupuestos.FirstOrDefault(p => p.CategoriaId == kv.Key)?.Categoria;
            return new GastoPorCategoriaDto
            {
                CategoriaId        = kv.Key,
                NombreCategoria    = cat?.Nombre ?? kv.Key.ToString(),
                Color              = cat?.Color,
                Total              = kv.Value,
                PorcentajeDelTotal = totalGastos > 0 ? Math.Round(kv.Value / totalGastos * 100, 2) : 0
            };
        }).OrderByDescending(c => c.Total).ToList();

        return new BalanceMensualDto
        {
            HogarId  = hogarId,
            Anio     = anio,
            Mes      = mes,
            TotalIngresos = totalIngresos,
            TotalGastos   = totalGastos,
            Balance       = totalIngresos - totalGastos,
            VariacionIngresosVsMesAnteriorPct = ingresosAnt > 0
                ? Math.Round((totalIngresos - ingresosAnt) / ingresosAnt * 100, 2) : 0,
            VariacionGastosVsMesAnteriorPct = gastosAnt > 0
                ? Math.Round((totalGastos - gastosAnt) / gastosAnt * 100, 2) : 0,
            GastosPorCategoria = catDetalle
        };
    }

    public async Task<PuntajeFinancieroDto> ObtenerPuntajeFinancieroAsync(Guid hogarId, CancellationToken ct)
    {
        var hoy = DateTime.UtcNow;
        var totalIngresos = await _ingresos.ObtenerTotalMensualAsync(hogarId, hoy.Year, hoy.Month, ct);
        var totalGastos   = await _gastos.ObtenerTotalMensualAsync(hogarId, hoy.Year, hoy.Month, ct);
        var presupuestos  = await _presupuestos.ObtenerPorHogarYMesAsync(hogarId, hoy.Year, hoy.Month, ct);
        var servicios     = await _servicios.ObtenerPorHogarAsync(hogarId, ct);
        var ingresosMes   = await _ingresos.ObtenerRecurrentesPorHogarAsync(hogarId, ct);

        var gastoPorCat   = await _gastos.ObtenerTotalesPorCategoriaAsync(hogarId, hoy.Year, hoy.Month, ct);
        var hayDominante  = totalGastos > 0 && gastoPorCat.Values.Any(v => v / totalGastos > 0.4m);

        var catEnPresupuesto = presupuestos.Count(p => p.MontoGastado <= p.MontoLimite);
        var serviciosOk = !servicios.Any(s => s.EstaVencido);
        var ingresoRec  = ingresosMes.Sum(i => i.MontoEnUSD ?? i.Monto);
        var ahorro = Math.Max(0, totalIngresos - totalGastos);

        var puntaje = PuntajeFinancieroCalculator.Calcular(
            totalIngresos, totalGastos, ahorro,
            catEnPresupuesto, presupuestos.Count,
            serviciosOk, ingresoRec, hayDominante);

        var nivel = puntaje switch
        {
            >= 80 => "Excelente",
            >= 60 => "Bueno",
            >= 40 => "Regular",
            _     => "Critico"
        };

        var recomendaciones = new List<string>();
        if (totalIngresos > 0 && ahorro / totalIngresos < 0.10m)
            recomendaciones.Add("Intenta ahorrar al menos el 10% de tus ingresos cada mes.");
        if (hayDominante)
            recomendaciones.Add("Una categoría representa más del 40% de tus gastos. Considera diversificar.");
        if (!serviciosOk)
            recomendaciones.Add("Tienes servicios básicos vencidos. Paga a tiempo para evitar cargos extras.");
        if (presupuestos.Any(p => p.MontoGastado > p.MontoLimite))
            recomendaciones.Add("Has superado el límite en algunas categorías. Ajusta tu presupuesto.");

        return new PuntajeFinancieroDto
        {
            HogarId = hogarId,
            Puntaje = puntaje,
            Nivel   = nivel,
            TasaAhorro = totalIngresos > 0 ? Math.Round(ahorro / totalIngresos, 4) : 0,
            ServiciosPagadosPuntual = serviciosOk,
            Recomendaciones = recomendaciones
        };
    }

    public async Task<List<(int Anio, int Mes, decimal Ingresos, decimal Gastos, decimal Balance)>> ObtenerTendenciasAsync(
        Guid hogarId, int meses, CancellationToken ct)
    {
        var tendIngresos = await _ingresos.ObtenerTendenciasAsync(hogarId, meses, ct);
        var tendGastos   = await _gastos.ObtenerTendenciasAsync(hogarId, meses, ct);

        var resultado = tendIngresos.Select(ti =>
        {
            var gastos = tendGastos.FirstOrDefault(tg => tg.Anio == ti.Anio && tg.Mes == ti.Mes);
            return (ti.Anio, ti.Mes, ti.Total, gastos.Total, ti.Total - gastos.Total);
        }).ToList();

        return resultado;
    }
}
