using FinanceHogar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHogar.Infrastructure.Data.Configurations;

public class HogarConfiguration : IEntityTypeConfiguration<Hogar>
{
    public void Configure(EntityTypeBuilder<Hogar> builder)
    {
        builder.ToTable("Hogares");
        builder.HasKey(h => h.Id);
        builder.Property(h => h.Nombre).HasMaxLength(150).IsRequired();
        builder.Property(h => h.Descripcion).HasMaxLength(500);
        builder.Property(h => h.Pais).HasMaxLength(100).HasDefaultValue("El Salvador");
        builder.Property(h => h.Departamento).HasMaxLength(100);
        builder.Property(h => h.Municipio).HasMaxLength(100);
        builder.Property(h => h.MonedaPrincipal).HasConversion<int>();
        builder.Property(h => h.PresupuestoMensualTotal).HasColumnType("decimal(18,2)");
        builder.HasIndex(h => h.Departamento);
    }
}
