using FinanceHogar.Domain.Entities;
using FinanceHogar.Infrastructure.Data;
using FinanceHogar.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FinanceHogar.Infrastructure.Repositories;

public class UsuariosRepository : IUsuariosRepository
{
    private readonly AppDbContext _db;
    public UsuariosRepository(AppDbContext db) => _db = db;

    public Task<Usuario?> ObtenerPorIdAsync(Guid id, CancellationToken ct)
        => _db.Usuarios
              .Include(u => u.HogarUsuarios)
              .FirstOrDefaultAsync(u => u.Id == id, ct);

    public Task<Usuario?> ObtenerPorEmailAsync(string email, CancellationToken ct)
        => _db.Usuarios
              .Include(u => u.HogarUsuarios)
              .FirstOrDefaultAsync(u => u.Email == email.ToLowerInvariant(), ct);

    public Task<Usuario?> ObtenerPorRefreshTokenAsync(string refreshToken, CancellationToken ct)
        => _db.Usuarios
              .Include(u => u.HogarUsuarios)
              .FirstOrDefaultAsync(u => u.RefreshToken == refreshToken
                                     && u.RefreshTokenExpiry > DateTime.UtcNow, ct);

    public async Task<Usuario> AgregarAsync(Usuario usuario, CancellationToken ct)
    {
        usuario.Email = usuario.Email.ToLowerInvariant();
        await _db.Usuarios.AddAsync(usuario, ct);
        await _db.SaveChangesAsync(ct);
        return usuario;
    }

    public async Task<Usuario> ActualizarAsync(Usuario usuario, CancellationToken ct)
    {
        _db.Usuarios.Update(usuario);
        await _db.SaveChangesAsync(ct);
        return usuario;
    }

    public Task<bool> ExisteEmailAsync(string email, CancellationToken ct)
        => _db.Usuarios.AnyAsync(u => u.Email == email.ToLowerInvariant(), ct);
}
