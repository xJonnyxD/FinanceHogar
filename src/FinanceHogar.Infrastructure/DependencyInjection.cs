using FinanceHogar.Application.Interfaces;
using FinanceHogar.Application.Interfaces.Repositories;
using FinanceHogar.Infrastructure.Repositories;
using FinanceHogar.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceHogar.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IHogaresRepository, HogaresRepository>();
        services.AddScoped<IUsuariosRepository, UsuariosRepository>();
        services.AddScoped<IIngresosRepository, IngresosRepository>();
        services.AddScoped<IGastosRepository, GastosRepository>();
        services.AddScoped<IServiciosBasicosRepository, ServiciosBasicosRepository>();
        services.AddScoped<IAlertasRepository, AlertasRepository>();
        services.AddScoped<IPresupuestosRepository, PresupuestosRepository>();
        services.AddScoped<ITandasRepository, TandasRepository>();
        services.AddScoped<IRemesasRepository, RemesasRepository>();
        return services;
    }
}
