using FinanceHogar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHogar.Infrastructure.Data.Configurations;

public class GastoConfiguration : IEntityTypeConfiguration<Gasto>
{
    public void Configure(EntityTypeBuilder<Gasto> builder)
    {
        builder.ToTable("Gastos");
        builder.HasKey(g => g.Id);
        builder.Property(g => g.Monto).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(g => g.MontoEnUSD).HasColumnType("decimal(18,2)");
        builder.Property(g => g.Descripcion).HasMaxLength(500);
        builder.Property(g => g.Comprobante).HasMaxLength(500);
        builder.Property(g => g.Tipo).HasConversion<int>();
        builder.Property(g => g.Moneda).HasConversion<int>();
        builder.Property(g => g.Frecuencia).HasConversion<int?>();

        builder.HasOne(g => g.Usuario)
            .WithMany(u => u.Gastos)
            .HasForeignKey(g => g.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(g => g.Hogar)
            .WithMany()
            .HasForeignKey(g => g.HogarId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(g => g.Categoria)
            .WithMany(c => c.Gastos)
            .HasForeignKey(g => g.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(g => g.ServicioBasico)
            .WithMany(s => s.Pagos)
            .HasForeignKey(g => g.ServicioBasicoId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(g => g.UsuarioId);
        builder.HasIndex(g => g.HogarId);
        builder.HasIndex(g => g.CategoriaId);
        builder.HasIndex(g => g.FechaGasto);
        builder.HasIndex(g => new { g.HogarId, g.FechaGasto });
    }
}
