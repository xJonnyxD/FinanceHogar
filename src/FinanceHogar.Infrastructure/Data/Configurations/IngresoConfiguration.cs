using FinanceHogar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHogar.Infrastructure.Data.Configurations;

public class IngresoConfiguration : IEntityTypeConfiguration<Ingreso>
{
    public void Configure(EntityTypeBuilder<Ingreso> builder)
    {
        builder.ToTable("Ingresos");
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Monto).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(i => i.MontoEnUSD).HasColumnType("decimal(18,2)");
        builder.Property(i => i.Descripcion).HasMaxLength(500);
        builder.Property(i => i.Tipo).HasConversion<int>();
        builder.Property(i => i.Moneda).HasConversion<int>();
        builder.Property(i => i.Frecuencia).HasConversion<int?>();

        builder.HasOne(i => i.Usuario)
            .WithMany(u => u.Ingresos)
            .HasForeignKey(i => i.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Hogar)
            .WithMany()
            .HasForeignKey(i => i.HogarId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Categoria)
            .WithMany(c => c.Ingresos)
            .HasForeignKey(i => i.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Remesa)
            .WithMany(r => r.Ingresos)
            .HasForeignKey(i => i.RemesaId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(i => i.UsuarioId);
        builder.HasIndex(i => i.HogarId);
        builder.HasIndex(i => i.CategoriaId);
        builder.HasIndex(i => i.FechaIngreso);
        builder.HasIndex(i => new { i.HogarId, i.FechaIngreso });
    }
}
