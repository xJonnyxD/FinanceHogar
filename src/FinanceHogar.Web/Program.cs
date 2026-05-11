using Blazored.LocalStorage;
using FinanceHogar.Web;
using FinanceHogar.Web.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7044/")
});

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, FhAuthStateProvider>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ApiService>();
builder.Services.AddSingleton<ToastService>();

var host = builder.Build();

// Initialize auth (set bearer token if stored)
var auth = host.Services.GetRequiredService<AuthService>();
await auth.InitializeAsync();

await host.RunAsync();
