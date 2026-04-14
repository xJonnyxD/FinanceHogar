using FinanceHogar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHogar.Infrastructure.Data.Configurations;

public class PresupuestoMensualConfiguration : IEntityTypeConfiguration<PresupuestoMensual>
{
    public void Configure(EntityTypeBuilder<PresupuestoMensual> builder)
    {
        builder.ToTable("PresupuestosMensuales");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.MontoLimite).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(p => p.MontoGastado).HasColumnType("decimal(18,2)");
        builder.Ignore(p => p.PorcentajeUso);  // propiedad calculada, no columna
        builder.HasIndex(p => new { p.HogarId, p.CategoriaId, p.Anio, p.Mes }).IsUnique();

        builder.HasOne(p => p.Hogar)
            .WithMany(h => h.Presupuestos)
            .HasForeignKey(p => p.HogarId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Categoria)
            .WithMany(c => c.Presupuestos)
            .HasForeignKey(p => p.CategoriaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(p => new { p.Anio, p.Mes });
    }
}
