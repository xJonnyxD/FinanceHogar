using FinanceHogar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHogar.Infrastructure.Data.Configurations;

public class ServicioBasicoConfiguration : IEntityTypeConfiguration<ServicioBasico>
{
    public void Configure(EntityTypeBuilder<ServicioBasico> builder)
    {
        builder.ToTable("ServiciosBasicos");
        builder.HasKey(s => s.Id);
        builder.Property(s => s.NombreProveedor).HasMaxLength(100).IsRequired();
        builder.Property(s => s.MontoUltimoPago).HasColumnType("decimal(18,2)");
        builder.Property(s => s.MontoPromedio).HasColumnType("decimal(18,2)");
        builder.Property(s => s.NumeroCuenta).HasMaxLength(100);
        builder.Property(s => s.TipoServicio).HasConversion<int>();

        builder.HasOne(s => s.Hogar)
            .WithMany()
            .HasForeignKey(s => s.HogarId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(s => s.Usuario)
            .WithMany(u => u.ServiciosBasicos)
            .HasForeignKey(s => s.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(s => s.HogarId);
        builder.HasIndex(s => s.UsuarioId);
        builder.HasIndex(s => s.FechaVencimiento);
        builder.HasIndex(s => s.TipoServicio);
    }
}
