using FinanceHogar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHogar.Infrastructure.Data.Configurations;

public class RolConfiguration : IEntityTypeConfiguration<Rol>
{
    public void Configure(EntityTypeBuilder<Rol> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Nombre).HasMaxLength(50).IsRequired();
        builder.Property(r => r.Descripcion).HasMaxLength(200);
        builder.HasIndex(r => r.Nombre).IsUnique();

        var seed = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        builder.HasData(
            new Rol { Id = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000001"), Nombre = "Admin",  Descripcion = "Administrador del hogar con acceso total",  CreatedAt = seed },
            new Rol { Id = Guid.Parse("a1b2c3d4-0001-0001-0001-000000000002"), Nombre = "Miembro", Descripcion = "Miembro del hogar con acceso limitado", CreatedAt = seed }
        );
    }
}
