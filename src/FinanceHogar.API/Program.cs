using System.Text;
using System.Threading.RateLimiting;
using FinanceHogar.API.Middleware;
using FinanceHogar.Application;
using FinanceHogar.Infrastructure;
using FinanceHogar.Infrastructure.Data;
using FinanceHogar.Application.Validators.Auth;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// ── Serilog ──────────────────────────────────────────────────────────────────
builder.Host.UseSerilog((ctx, lc) =>
    lc.ReadFrom.Configuration(ctx.Configuration));

// ── Database ─────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ── Application + Infrastructure services ────────────────────────────────────
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

// ── JWT Authentication ────────────────────────────────────────────────────────
var jwtSection = builder.Configuration.GetSection("JwtSettings");
var secretKey   = jwtSection["SecretKey"]!;
var issuer      = jwtSection["Issuer"]!;
var audience    = jwtSection["Audience"]!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = issuer,
            ValidAudience            = audience,
            IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ClockSkew                = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("AdminHogar", policy =>
        policy.RequireClaim("EsAdministrador", "true"));
});

// ── CORS ──────────────────────────────────────────────────────────────────────
var allowedOrigins = builder.Configuration["AllowedOrigins"]?
    .Split(',', StringSplitOptions.RemoveEmptyEntries) ?? [];

builder.Services.AddCors(opt =>
    opt.AddDefaultPolicy(p =>
        p.WithOrigins(allowedOrigins)
         .AllowAnyHeader()
         .AllowAnyMethod()));

// ── Rate Limiting ─────────────────────────────────────────────────────────────
builder.Services.AddRateLimiter(opt =>
{
    opt.AddFixedWindowLimiter("Fixed", lim =>
    {
        lim.Window            = TimeSpan.FromMinutes(1);
        lim.PermitLimit       = 100;
        lim.QueueLimit        = 0;
        lim.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
    opt.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
});

// ── Controllers + FluentValidation ───────────────────────────────────────────
builder.Services.AddControllers();
// FluentValidation: auto-valida el ModelState y retorna 400 con errores descriptivos
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>(); // registra todos los validators de Application

// ── Swagger / OpenAPI ─────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "FinanceHogar API",
        Version     = "v1",
        Description = "Plataforma de Control Financiero para el Hogar — El Salvador"
    });

    var secDef = new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.Http,
        Scheme       = "bearer",
        BearerFormat = "JWT",
        In           = ParameterLocation.Header,
        Description  = "Ingrese el token JWT. Ejemplo: Bearer {token}"
    };
    c.AddSecurityDefinition("Bearer", secDef);
    c.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
    {
        { new OpenApiSecuritySchemeReference("Bearer"), new List<string>() }
    });
});

// ── Health Checks ─────────────────────────────────────────────────────────────
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("DefaultConnection")!);

// ─────────────────────────────────────────────────────────────────────────────
var app = builder.Build();

// ── Auto-migrate in Development ───────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FinanceHogar API v1");
        c.RoutePrefix = "swagger"; // Swagger en /swagger — raíz libre para el frontend
    });
}

// ── Middleware pipeline ───────────────────────────────────────────────────────
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseCors();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

// Servir archivos estáticos del frontend (wwwroot/) sin caché para JS/HTML
app.UseDefaultFiles();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        var ext = Path.GetExtension(ctx.File.Name);
        if (ext is ".js" or ".html" or ".css")
        {
            ctx.Context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
            ctx.Context.Response.Headers["Pragma"] = "no-cache";
        }
    }
});

app.MapControllers().RequireRateLimiting("Fixed");
app.MapHealthChecks("/health");

// SPA fallback: cualquier ruta no-API → index.html
app.MapFallbackToFile("index.html");

app.Run();
