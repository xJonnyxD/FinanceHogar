using FinanceHogar.Domain.Entities;
using FinanceHogar.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FinanceHogar.Infrastructure.Data;

/// <summary>Inserta datos de demostración en entorno de desarrollo.</summary>
public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        var rolAdminId   = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000001");
        var rolMiembroId = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000002");
        var now = DateTime.UtcNow;

        // ── 1. Usuarios y hogar demo (solo si no existen) ─────────────────────
        Guid hogarId;
        Guid adminId;

        if (!await db.Usuarios.AnyAsync())
        {
            hogarId = Guid.Parse("bbbbbbbb-0001-0001-0001-000000000001");
            adminId = Guid.Parse("aaaaaaaa-0001-0001-0001-000000000001");
            var usuarioId = Guid.Parse("aaaaaaaa-0001-0001-0001-000000000002");

            var hogar   = new Hogar   { Id = hogarId,   Nombre = "Familia García Demo", CreatedAt = now };
            var admin   = new Usuario { Id = adminId,   NombreCompleto = "Salvador García",  Email = "admin@financehogar.com",   PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin1234!"),  CreatedAt = now };
            var miembro = new Usuario { Id = usuarioId, NombreCompleto = "María García",     Email = "usuario@financehogar.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("User1234!"),   CreatedAt = now };

            await db.Hogares.AddAsync(hogar);
            await db.Usuarios.AddRangeAsync(admin, miembro);
            await db.HogarUsuarios.AddRangeAsync(
                new HogarUsuario { Id = Guid.Parse("cccccccc-0001-0001-0001-000000000001"), HogarId = hogarId, UsuarioId = adminId,   RolId = rolAdminId,   EsAdministrador = true,  CreatedAt = now },
                new HogarUsuario { Id = Guid.Parse("cccccccc-0001-0001-0001-000000000002"), HogarId = hogarId, UsuarioId = usuarioId, RolId = rolMiembroId, EsAdministrador = false, CreatedAt = now }
            );
            await db.SaveChangesAsync();
        }
        else
        {
            // Usar el primer hogar existente
            var hu = await db.HogarUsuarios.Include(h => h.Hogar).FirstOrDefaultAsync();
            if (hu is null) return;
            hogarId = hu.HogarId;
            adminId = hu.UsuarioId;
        }

        // ── 2. Datos demo (solo si el hogar no tiene gastos) ──────────────────
        if (await db.Gastos.AnyAsync(g => g.HogarId == hogarId)) return;

        // Categorías globales (ya seeded por HasData)
        var catIds = await db.Categorias
            .Where(c => c.EsGlobal)
            .Select(c => new { c.Id, c.Nombre, c.EsIngreso })
            .ToListAsync();

        Guid Cat(string nombre) => catIds.First(c => c.Nombre == nombre).Id;

        // ── Ingresos (últimos 3 meses) ────────────────────────────────────────
        var ingresos = new List<Ingreso>();
        for (int m = 3; m >= 0; m--)
        {
            var fecha = DateOnly.FromDateTime(DateTime.Today.AddMonths(-m));
            ingresos.AddRange(new[]
            {
                new Ingreso { Id = Guid.NewGuid(), HogarId = hogarId, UsuarioId = adminId, CategoriaId = Cat("Salario"),          Monto = 850m, Moneda = TipoMoneda.USD, Tipo = TipoIngreso.Salario,        FechaIngreso = fecha.AddDays(1),  EsRecurrente = true,  Frecuencia = TipoFrecuencia.Mensual, Descripcion = "Salario mensual",   CreatedAt = now },
                new Ingreso { Id = Guid.NewGuid(), HogarId = hogarId, UsuarioId = adminId, CategoriaId = Cat("Remesa"),           Monto = 200m, Moneda = TipoMoneda.USD, Tipo = TipoIngreso.Remesa,         FechaIngreso = fecha.AddDays(5),  EsRecurrente = false,                                              Descripcion = "Remesa de EE.UU.",  CreatedAt = now },
                new Ingreso { Id = Guid.NewGuid(), HogarId = hogarId, UsuarioId = adminId, CategoriaId = Cat("Trabajo Informal"), Monto = 120m, Moneda = TipoMoneda.USD, Tipo = TipoIngreso.IngresoInformal, FechaIngreso = fecha.AddDays(15), EsRecurrente = false,                                              Descripcion = "Trabajos extras",   CreatedAt = now },
            });
        }

        // ── Gastos (últimos 3 meses) ──────────────────────────────────────────
        var gastos = new List<Gasto>();
        for (int m = 3; m >= 0; m--)
        {
            var fecha = DateOnly.FromDateTime(DateTime.Today.AddMonths(-m));
            gastos.AddRange(new[]
            {
                new Gasto { Id = Guid.NewGuid(), HogarId = hogarId, UsuarioId = adminId, CategoriaId = Cat("Alimentacion"),      Monto = 180m, Moneda = TipoMoneda.USD, Tipo = TipoGasto.Alimentacion,    FechaGasto = fecha.AddDays(3),  EsRecurrente = false,                                              Descripcion = "Supermercado La Colonia", CreatedAt = now },
                new Gasto { Id = Guid.NewGuid(), HogarId = hogarId, UsuarioId = adminId, CategoriaId = Cat("Servicios Basicos"), Monto = 45m,  Moneda = TipoMoneda.USD, Tipo = TipoGasto.ServicioBasico,  FechaGasto = fecha.AddDays(5),  EsRecurrente = true,  Frecuencia = TipoFrecuencia.Mensual, Descripcion = "Electricidad CAESS",     CreatedAt = now },
                new Gasto { Id = Guid.NewGuid(), HogarId = hogarId, UsuarioId = adminId, CategoriaId = Cat("Transporte"),        Monto = 60m,  Moneda = TipoMoneda.USD, Tipo = TipoGasto.Transporte,      FechaGasto = fecha.AddDays(8),  EsRecurrente = false,                                              Descripcion = "Gasolina y bus",         CreatedAt = now },
                new Gasto { Id = Guid.NewGuid(), HogarId = hogarId, UsuarioId = adminId, CategoriaId = Cat("Educacion"),         Monto = 80m,  Moneda = TipoMoneda.USD, Tipo = TipoGasto.Educacion,       FechaGasto = fecha.AddDays(10), EsRecurrente = true,  Frecuencia = TipoFrecuencia.Mensual, Descripcion = "Colegiatura",            CreatedAt = now },
                new Gasto { Id = Guid.NewGuid(), HogarId = hogarId, UsuarioId = adminId, CategoriaId = Cat("Salud"),             Monto = 35m,  Moneda = TipoMoneda.USD, Tipo = TipoGasto.Salud,           FechaGasto = fecha.AddDays(12), EsRecurrente = false,                                              Descripcion = "Medicamentos farmacia",  CreatedAt = now },
                new Gasto { Id = Guid.NewGuid(), HogarId = hogarId, UsuarioId = adminId, CategoriaId = Cat("Entretenimiento"),   Monto = 25m,  Moneda = TipoMoneda.USD, Tipo = TipoGasto.Entretenimiento, FechaGasto = fecha.AddDays(20), EsRecurrente = false,                                              Descripcion = "Cine y salidas",         CreatedAt = now },
                new Gasto { Id = Guid.NewGuid(), HogarId = hogarId, UsuarioId = adminId, CategoriaId = Cat("Alimentacion"),      Monto = 95m,  Moneda = TipoMoneda.USD, Tipo = TipoGasto.Alimentacion,    FechaGasto = fecha.AddDays(22), EsRecurrente = false,                                              Descripcion = "Pupusería y comidas",    CreatedAt = now },
            });
        }

        // ── Presupuestos del mes actual ───────────────────────────────────────
        var hoy = DateTime.Today;
        var presupuestos = new[]
        {
            new PresupuestoMensual { Id = Guid.NewGuid(), HogarId = hogarId, CategoriaId = Cat("Alimentacion"),      MontoLimite = 300m, Anio = hoy.Year, Mes = hoy.Month, CreatedAt = now },
            new PresupuestoMensual { Id = Guid.NewGuid(), HogarId = hogarId, CategoriaId = Cat("Transporte"),        MontoLimite = 80m,  Anio = hoy.Year, Mes = hoy.Month, CreatedAt = now },
            new PresupuestoMensual { Id = Guid.NewGuid(), HogarId = hogarId, CategoriaId = Cat("Educacion"),         MontoLimite = 100m, Anio = hoy.Year, Mes = hoy.Month, CreatedAt = now },
            new PresupuestoMensual { Id = Guid.NewGuid(), HogarId = hogarId, CategoriaId = Cat("Entretenimiento"),   MontoLimite = 50m,  Anio = hoy.Year, Mes = hoy.Month, CreatedAt = now },
            new PresupuestoMensual { Id = Guid.NewGuid(), HogarId = hogarId, CategoriaId = Cat("Servicios Basicos"), MontoLimite = 60m,  Anio = hoy.Year, Mes = hoy.Month, CreatedAt = now },
        };

        // ── Servicios básicos ─────────────────────────────────────────────────
        var servicios = new[]
        {
            new ServicioBasico { Id = Guid.NewGuid(), HogarId = hogarId, UsuarioId = adminId, TipoServicio = TipoServicio.ElectricidadAES, NombreProveedor = "CAESS",             MontoUltimoPago = 45m, MontoPromedio = 45m, FechaVencimiento = DateOnly.FromDateTime(hoy.AddDays(10)), DiasAnticipacionNotificacion = 5, NotificacionActiva = true, CreatedAt = now },
            new ServicioBasico { Id = Guid.NewGuid(), HogarId = hogarId, UsuarioId = adminId, TipoServicio = TipoServicio.Agua,            NombreProveedor = "ANDA",              MontoUltimoPago = 15m, MontoPromedio = 15m, FechaVencimiento = DateOnly.FromDateTime(hoy.AddDays(5)),  DiasAnticipacionNotificacion = 5, NotificacionActiva = true, CreatedAt = now },
            new ServicioBasico { Id = Guid.NewGuid(), HogarId = hogarId, UsuarioId = adminId, TipoServicio = TipoServicio.InternetClaro,   NombreProveedor = "Claro El Salvador", MontoUltimoPago = 30m, MontoPromedio = 30m, FechaVencimiento = DateOnly.FromDateTime(hoy.AddDays(15)), DiasAnticipacionNotificacion = 5, NotificacionActiva = true, CreatedAt = now },
        };

        // ── Remesa demo ───────────────────────────────────────────────────────
        var remesas = new[]
        {
            new Remesa { Id = Guid.NewGuid(), HogarId = hogarId, ReceptorId = adminId, Monto = 200m, Moneda = TipoMoneda.USD, PaisOrigen = "Estados Unidos", Empresa = "Western Union", NumeroConfirmacion = "WU-2026-001", FechaRecibida = DateOnly.FromDateTime(hoy.AddDays(-15)), CreatedAt = now },
        };

        await db.Ingresos.AddRangeAsync(ingresos);
        await db.Gastos.AddRangeAsync(gastos);
        await db.PresupuestosMensuales.AddRangeAsync(presupuestos);
        await db.ServiciosBasicos.AddRangeAsync(servicios);
        await db.Remesas.AddRangeAsync(remesas);
        await db.SaveChangesAsync();
    }
}
