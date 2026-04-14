using FinanceHogar.Application.Interfaces;
using FinanceHogar.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceHogar.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IIngresosService,        IngresosService>();
        services.AddScoped<IGastosService,          GastosService>();
        services.AddScoped<IReportesService,        ReportesService>();
        services.AddScoped<IHogaresService,         HogaresService>();
        services.AddScoped<IAlertasService,         AlertasService>();
        services.AddScoped<IPresupuestosService,    PresupuestosService>();
        services.AddScoped<IServiciosBasicosService, ServiciosBasicosService>();
        services.AddScoped<ITandasService,          TandasService>();
        services.AddScoped<IRemesasService,         RemesasService>();
        return services;
    }
}
