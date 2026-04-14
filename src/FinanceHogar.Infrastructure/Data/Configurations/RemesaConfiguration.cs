using FinanceHogar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHogar.Infrastructure.Data.Configurations;

public class RemesaConfiguration : IEntityTypeConfiguration<Remesa>
{
    public void Configure(EntityTypeBuilder<Remesa> builder)
    {
        builder.ToTable("Remesas");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Monto).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(r => r.ComisionCobrada).HasColumnType("decimal(18,2)");
        builder.Property(r => r.PaisOrigen).HasMaxLength(100);
        builder.Property(r => r.NombreRemitente).HasMaxLength(200);
        builder.Property(r => r.Empresa).HasMaxLength(100);
        builder.Property(r => r.NumeroConfirmacion).HasMaxLength(100);
        builder.Property(r => r.Proposito).HasMaxLength(300);
        builder.Property(r => r.Moneda).HasConversion<int>();

        builder.HasOne(r => r.Hogar)
            .WithMany(h => h.Remesas)
            .HasForeignKey(r => r.HogarId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Receptor)
            .WithMany(u => u.RemesasRecibidas)
            .HasForeignKey(r => r.ReceptorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(r => r.HogarId);
        builder.HasIndex(r => r.ReceptorId);
        builder.HasIndex(r => r.FechaRecibida);
    }
}
