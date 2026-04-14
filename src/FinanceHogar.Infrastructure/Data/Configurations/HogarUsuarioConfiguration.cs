using FinanceHogar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHogar.Infrastructure.Data.Configurations;

public class HogarUsuarioConfiguration : IEntityTypeConfiguration<HogarUsuario>
{
    public void Configure(EntityTypeBuilder<HogarUsuario> builder)
    {
        builder.ToTable("HogarUsuarios");
        builder.HasKey(hu => hu.Id);
        builder.HasIndex(hu => new { hu.HogarId, hu.UsuarioId }).IsUnique();

        builder.HasOne(hu => hu.Hogar)
            .WithMany(h => h.HogarUsuarios)
            .HasForeignKey(hu => hu.HogarId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(hu => hu.Usuario)
            .WithMany(u => u.HogarUsuarios)
            .HasForeignKey(hu => hu.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(hu => hu.Rol)
            .WithMany(r => r.HogarUsuarios)
            .HasForeignKey(hu => hu.RolId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(hu => hu.HogarId);
        builder.HasIndex(hu => hu.UsuarioId);
    }
}
