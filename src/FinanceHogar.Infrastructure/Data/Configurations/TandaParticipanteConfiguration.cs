using FinanceHogar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceHogar.Infrastructure.Data.Configurations;

public class TandaParticipanteConfiguration : IEntityTypeConfiguration<TandaParticipante>
{
    public void Configure(EntityTypeBuilder<TandaParticipante> builder)
    {
        builder.ToTable("TandaParticipantes");
        builder.HasKey(tp => tp.Id);
        builder.HasIndex(tp => new { tp.TandaId, tp.UsuarioId }).IsUnique();
        builder.HasIndex(tp => new { tp.TandaId, tp.NumeroTurno }).IsUnique();

        builder.HasOne(tp => tp.Tanda)
            .WithMany(t => t.Participantes)
            .HasForeignKey(tp => tp.TandaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(tp => tp.Usuario)
            .WithMany()
            .HasForeignKey(tp => tp.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(tp => tp.TandaId);
        builder.HasIndex(tp => tp.UsuarioId);
    }
}
