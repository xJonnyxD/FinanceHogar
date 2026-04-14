using FinanceHogar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHogar.Infrastructure.Data.Configurations;

public class TandaConfiguration : IEntityTypeConfiguration<Tanda>
{
    public void Configure(EntityTypeBuilder<Tanda> builder)
    {
        builder.ToTable("Tandas");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Nombre).HasMaxLength(150).IsRequired();
        builder.Property(t => t.CuotaMensual).HasColumnType("decimal(18,2)").IsRequired();
        builder.Property(t => t.Frecuencia).HasConversion<int>();
        builder.Property(t => t.Estado).HasConversion<int>();

        builder.HasOne(t => t.Hogar)
            .WithMany(h => h.Tandas)
            .HasForeignKey(t => t.HogarId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Organizador)
            .WithMany()
            .HasForeignKey(t => t.OrganizadorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(t => t.HogarId);
        builder.HasIndex(t => t.Estado);
    }
}
