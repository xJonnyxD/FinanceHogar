using FinanceHogar.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceHogar.Infrastructure.Data;

/// <summary>
/// Inserta datos de demostración cuando la base de datos está vacía.
/// Se ejecuta solo en entornos de desarrollo.
/// </summary>
public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        // Si ya hay usuarios no hacemos nada
        if (await db.Usuarios.AnyAsync()) return;

        var rolAdminId  = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000001");
        var rolMiembroId = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000002");
        var now = DateTime.UtcNow;

        // ── Hogar demo ────────────────────────────────────────────────────────
        var hogar = new Hogar
        {
            Id        = Guid.Parse("bbbbbbbb-0001-0001-0001-000000000001"),
            Nombre    = "Familia Demo",
            CreatedAt = now
        };

        // ── Usuarios demo ─────────────────────────────────────────────────────
        var admin = new Usuario
        {
            Id             = Guid.Parse("aaaaaaaa-0001-0001-0001-000000000001"),
            NombreCompleto = "Administrador Demo",
            Email          = "admin@financehogar.com",
            PasswordHash   = BCrypt.Net.BCrypt.HashPassword("Admin1234!"),
            CreatedAt      = now
        };

        var usuario = new Usuario
        {
            Id             = Guid.Parse("aaaaaaaa-0001-0001-0001-000000000002"),
            NombreCompleto = "Usuario Demo",
            Email          = "usuario@financehogar.com",
            PasswordHash   = BCrypt.Net.BCrypt.HashPassword("User1234!"),
            CreatedAt      = now
        };

        // ── HogarUsuarios ─────────────────────────────────────────────────────
        var huAdmin = new HogarUsuario
        {
            Id              = Guid.Parse("cccccccc-0001-0001-0001-000000000001"),
            HogarId         = hogar.Id,
            UsuarioId       = admin.Id,
            RolId           = rolAdminId,
            EsAdministrador = true,
            CreatedAt       = now
        };

        var huUsuario = new HogarUsuario
        {
            Id              = Guid.Parse("cccccccc-0001-0001-0001-000000000002"),
            HogarId         = hogar.Id,
            UsuarioId       = usuario.Id,
            RolId           = rolMiembroId,
            EsAdministrador = false,
            CreatedAt       = now
        };

        // Las categorías globales ya están seeded vía HasData en CategoriaConfiguration
        await db.Hogares.AddAsync(hogar);
        await db.Usuarios.AddRangeAsync(admin, usuario);
        await db.HogarUsuarios.AddRangeAsync(huAdmin, huUsuario);
        await db.SaveChangesAsync();
    }
}
