using FinanceHogar.Domain.Common;
using FinanceHogar.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace FinanceHogar.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Hogar>              Hogares               => Set<Hogar>();
    public DbSet<Usuario>            Usuarios              => Set<Usuario>();
    public DbSet<Rol>                Roles                 => Set<Rol>();
    public DbSet<HogarUsuario>       HogarUsuarios         => Set<HogarUsuario>();
    public DbSet<Categoria>          Categorias            => Set<Categoria>();
    public DbSet<Ingreso>            Ingresos              => Set<Ingreso>();
    public DbSet<Gasto>              Gastos                => Set<Gasto>();
    public DbSet<ServicioBasico>     ServiciosBasicos      => Set<ServicioBasico>();
    public DbSet<Alerta>             Alertas               => Set<Alerta>();
    public DbSet<PresupuestoMensual> PresupuestosMensuales => Set<PresupuestoMensual>();
    public DbSet<Tanda>              Tandas                => Set<Tanda>();
    public DbSet<TandaParticipante>  TandaParticipantes    => Set<TandaParticipante>();
    public DbSet<Remesa>             Remesas               => Set<Remesa>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Soft delete — todas las entidades ocultan registros eliminados
        modelBuilder.Entity<Hogar>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Usuario>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Rol>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<HogarUsuario>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Categoria>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Ingreso>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Gasto>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<ServicioBasico>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Alerta>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<PresupuestoMensual>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Tanda>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<TandaParticipante>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Remesa>().HasQueryFilter(e => !e.IsDeleted);

        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
                case EntityState.Deleted:
                    // Interceptar hard delete → soft delete
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    break;
            }
        }
        return await base.SaveChangesAsync(cancellationToken);
    }
}
