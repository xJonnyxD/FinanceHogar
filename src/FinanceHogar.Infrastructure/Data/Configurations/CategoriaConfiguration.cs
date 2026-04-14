using FinanceHogar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHogar.Infrastructure.Data.Configurations;

public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        builder.ToTable("Categorias");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Nombre).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Descripcion).HasMaxLength(300);
        builder.Property(c => c.Icono).HasMaxLength(50);
        builder.Property(c => c.Color).HasMaxLength(7);

        builder.HasOne(c => c.Hogar)
            .WithMany(h => h.Categorias)
            .HasForeignKey(c => c.HogarId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(c => c.HogarId);
        builder.HasIndex(c => c.EsIngreso);
        builder.HasIndex(c => c.EsGlobal);

        // Seed categorías globales
        var seed = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        builder.HasData(
            new Categoria { Id = Guid.Parse("c0000000-0000-0000-0000-000000000001"), Nombre = "Salario",           EsIngreso = true,  EsGlobal = true, Icono = "money-bill",     Color = "#2ECC71", CreatedAt = seed },
            new Categoria { Id = Guid.Parse("c0000000-0000-0000-0000-000000000002"), Nombre = "Remesa",            EsIngreso = true,  EsGlobal = true, Icono = "plane",          Color = "#3498DB", CreatedAt = seed },
            new Categoria { Id = Guid.Parse("c0000000-0000-0000-0000-000000000003"), Nombre = "Negocio Propio",    EsIngreso = true,  EsGlobal = true, Icono = "store",          Color = "#9B59B6", CreatedAt = seed },
            new Categoria { Id = Guid.Parse("c0000000-0000-0000-0000-000000000004"), Nombre = "Trabajo Informal",  EsIngreso = true,  EsGlobal = true, Icono = "handshake",      Color = "#E67E22", CreatedAt = seed },
            new Categoria { Id = Guid.Parse("c0000000-0000-0000-0000-000000000005"), Nombre = "Tanda",             EsIngreso = true,  EsGlobal = true, Icono = "users",          Color = "#1ABC9C", CreatedAt = seed },
            new Categoria { Id = Guid.Parse("c0000000-0000-0000-0000-000000000006"), Nombre = "Alimentacion",      EsIngreso = false, EsGlobal = true, Icono = "utensils",       Color = "#E74C3C", CreatedAt = seed },
            new Categoria { Id = Guid.Parse("c0000000-0000-0000-0000-000000000007"), Nombre = "Educacion",         EsIngreso = false, EsGlobal = true, Icono = "graduation-cap", Color = "#3498DB", CreatedAt = seed },
            new Categoria { Id = Guid.Parse("c0000000-0000-0000-0000-000000000008"), Nombre = "Salud",             EsIngreso = false, EsGlobal = true, Icono = "heartbeat",      Color = "#E91E63", CreatedAt = seed },
            new Categoria { Id = Guid.Parse("c0000000-0000-0000-0000-000000000009"), Nombre = "Transporte",        EsIngreso = false, EsGlobal = true, Icono = "bus",            Color = "#FF9800", CreatedAt = seed },
            new Categoria { Id = Guid.Parse("c0000000-0000-0000-0000-000000000010"), Nombre = "Servicios Basicos", EsIngreso = false, EsGlobal = true, Icono = "bolt",           Color = "#FFC107", CreatedAt = seed },
            new Categoria { Id = Guid.Parse("c0000000-0000-0000-0000-000000000011"), Nombre = "Entretenimiento",   EsIngreso = false, EsGlobal = true, Icono = "gamepad",        Color = "#9C27B0", CreatedAt = seed },
            new Categoria { Id = Guid.Parse("c0000000-0000-0000-0000-000000000012"), Nombre = "Ahorro",            EsIngreso = false, EsGlobal = true, Icono = "piggy-bank",     Color = "#4CAF50", CreatedAt = seed },
            new Categoria { Id = Guid.Parse("c0000000-0000-0000-0000-000000000013"), Nombre = "Cuota Tanda",       EsIngreso = false, EsGlobal = true, Icono = "users",          Color = "#00BCD4", CreatedAt = seed },
            new Categoria { Id = Guid.Parse("c0000000-0000-0000-0000-000000000014"), Nombre = "Otros",             EsIngreso = false, EsGlobal = true, Icono = "ellipsis-h",     Color = "#95A5A6", CreatedAt = seed }
        );
    }
}
