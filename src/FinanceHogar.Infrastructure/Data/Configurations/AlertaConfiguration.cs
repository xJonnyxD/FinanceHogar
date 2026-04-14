using FinanceHogar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHogar.Infrastructure.Data.Configurations;

public class AlertaConfiguration : IEntityTypeConfiguration<Alerta>
{
    public void Configure(EntityTypeBuilder<Alerta> builder)
    {
        builder.ToTable("Alertas");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Titulo).HasMaxLength(200).IsRequired();
        builder.Property(a => a.Mensaje).IsRequired();
        builder.Property(a => a.PorcentajeUso).HasColumnType("decimal(5,2)");
        builder.Property(a => a.Tipo).HasConversion<int>();
        builder.Property(a => a.Estado).HasConversion<int>();

        builder.HasOne(a => a.Hogar)
            .WithMany(h => h.Alertas)
            .HasForeignKey(a => a.HogarId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.Usuario)
            .WithMany()
            .HasForeignKey(a => a.UsuarioId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(a => a.Categoria)
            .WithMany()
            .HasForeignKey(a => a.CategoriaId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(a => a.HogarId);
        builder.HasIndex(a => a.Estado);
        builder.HasIndex(a => a.Tipo);
    }
}
