using System.Net.Http.Json;
using FinanceHogar.Web.Models;

namespace FinanceHogar.Web.Services;

public class ApiService(HttpClient http)
{
    // ── Hogares ──────────────────────────────────────────────
    public Task<HogarDto?> GetHogarAsync(Guid id) =>
        http.GetFromJsonAsync<HogarDto>($"api/hogares/{id}");

    // ── Categorías ───────────────────────────────────────────
    public Task<List<CategoriaDto>?> GetCategoriasAsync(Guid hogarId) =>
        http.GetFromJsonAsync<List<CategoriaDto>>($"api/categorias/hogar/{hogarId}");

    // ── Gastos ───────────────────────────────────────────────
    public Task<List<GastoDto>?> GetGastosAsync(Guid hogarId, DateOnly? desde = null, DateOnly? hasta = null)
    {
        var url = $"api/gastos?hogarId={hogarId}";
        if (desde.HasValue) url += $"&desde={desde.Value:yyyy-MM-dd}";
        if (hasta.HasValue) url += $"&hasta={hasta.Value:yyyy-MM-dd}";
        return http.GetFromJsonAsync<List<GastoDto>>(url);
    }
    public Task<GastoDto?> CreateGastoAsync(CreateGastoRequest req) =>
        PostAsync<GastoDto>("api/gastos", req);
    public Task<GastoDto?> UpdateGastoAsync(Guid id, UpdateGastoRequest req) =>
        PutAsync<GastoDto>($"api/gastos/{id}", req);
    public async Task DeleteGastoAsync(Guid id) =>
        await http.DeleteAsync($"api/gastos/{id}");

    // ── Ingresos ─────────────────────────────────────────────
    public Task<List<IngresoDto>?> GetIngresosAsync(Guid hogarId)=>
        http.GetFromJsonAsync<List<IngresoDto>>($"api/ingresos?hogarId={hogarId}");
    public Task<IngresoDto?> CreateIngresoAsync(CreateIngresoRequest req) =>
        PostAsync<IngresoDto>("api/ingresos", req);
    public async Task DeleteIngresoAsync(Guid id) =>
        await http.DeleteAsync($"api/ingresos/{id}");

    // ── Presupuestos ─────────────────────────────────────────
    public Task<List<PresupuestoDto>?> GetPresupuestosAsync(Guid hogarId, int anio, int mes) =>
        http.GetFromJsonAsync<List<PresupuestoDto>>($"api/presupuestos?hogarId={hogarId}&anio={anio}&mes={mes}");
    public Task<PresupuestoDto?> CreatePresupuestoAsync(CreatePresupuestoRequest req) =>
        PostAsync<PresupuestoDto>("api/presupuestos", req);
    public async Task DeletePresupuestoAsync(Guid id) =>
        await http.DeleteAsync($"api/presupuestos/{id}");

    // ── Alertas ──────────────────────────────────────────────
    public Task<List<AlertaDto>?> GetAlertasAsync(Guid hogarId) =>
        http.GetFromJsonAsync<List<AlertaDto>>($"api/alertas/hogar/{hogarId}");
    public async Task MarcarLeidaAsync(Guid id) =>
        await http.PutAsync($"api/alertas/{id}/leer", null);

    // ── Servicios Básicos ────────────────────────────────────
    public Task<List<ServicioBasicoDto>?> GetServiciosAsync(Guid hogarId) =>
        http.GetFromJsonAsync<List<ServicioBasicoDto>>($"api/serviciosbasicos?hogarId={hogarId}");
    public Task<ServicioBasicoDto?> CreateServicioAsync(CreateServicioRequest req) =>
        PostAsync<ServicioBasicoDto>("api/serviciosbasicos", req);
    public Task<ServicioBasicoDto?> UpdateServicioAsync(Guid id, UpdateServicioRequest req) =>
        PutAsync<ServicioBasicoDto>($"api/serviciosbasicos/{id}", req);
    public async Task DeleteServicioAsync(Guid id) =>
        await http.DeleteAsync($"api/serviciosbasicos/{id}");

    // ── Tandas ───────────────────────────────────────────────
    public Task<List<TandaDto>?> GetTandasAsync(Guid hogarId) =>
        http.GetFromJsonAsync<List<TandaDto>>($"api/tandas?hogarId={hogarId}");
    public Task<TandaDto?> CreateTandaAsync(CreateTandaRequest req) =>
        PostAsync<TandaDto>("api/tandas", req);
    public async Task DeleteTandaAsync(Guid id) =>
        await http.DeleteAsync($"api/tandas/{id}");

    // ── Remesas ──────────────────────────────────────────────
    public Task<List<RemesaDto>?> GetRemesasAsync(Guid hogarId) =>
        http.GetFromJsonAsync<List<RemesaDto>>($"api/remesas?hogarId={hogarId}");
    public Task<RemesaDto?> CreateRemesaAsync(CreateRemesaRequest req) =>
        PostAsync<RemesaDto>("api/remesas", req);
    public async Task DeleteRemesaAsync(Guid id) =>
        await http.DeleteAsync($"api/remesas/{id}");

    // ── Reportes ─────────────────────────────────────────────
    public Task<BalanceMensualDto?> GetBalanceAsync(Guid hogarId) =>
        http.GetFromJsonAsync<BalanceMensualDto>($"api/reportes/balance-mensual/{hogarId}");
    public Task<PuntajeFinancieroDto?> GetPuntajeAsync(Guid hogarId) =>
        http.GetFromJsonAsync<PuntajeFinancieroDto>($"api/reportes/puntaje-financiero/{hogarId}");
    public Task<List<TendenciaDto>?> GetTendenciasAsync(Guid hogarId) =>
        http.GetFromJsonAsync<List<TendenciaDto>>($"api/reportes/tendencias/{hogarId}");

    // ── Helpers ──────────────────────────────────────────────
    private async Task<T?> PostAsync<T>(string url, object body)
    {
        var resp = await http.PostAsJsonAsync(url, body);
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<T>();
    }
    private async Task<T?> PutAsync<T>(string url, object body)
    {
        var resp = await http.PutAsJsonAsync(url, body);
        resp.EnsureSuccessStatusCode();
        return await resp.Content.ReadFromJsonAsync<T>();
    }
}
