using FinanceHogar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHogar.Infrastructure.Data.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.NombreCompleto).HasMaxLength(200).IsRequired();
        builder.Property(u => u.Email).HasMaxLength(256).IsRequired();
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.Telefono).HasMaxLength(20);
        builder.Property(u => u.DUI).HasMaxLength(10);
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.DUI).IsUnique();
        builder.HasIndex(u => u.EstaActivo);
    }
}
