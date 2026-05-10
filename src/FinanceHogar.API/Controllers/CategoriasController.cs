using FinanceHogar.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceHogar.API.Controllers;

[Authorize]
public class CategoriasController : BaseController
{
    private readonly AppDbContext _db;
    public CategoriasController(AppDbContext db) => _db = db;

    /// <summary>Obtener todas las categorías globales + las del hogar indicado.</summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? hogarId, CancellationToken ct)
    {
        var query = _db.Categorias
            .Where(c => c.EsGlobal || (hogarId != null && c.HogarId == hogarId))
            .OrderBy(c => c.EsIngreso)
            .ThenBy(c => c.Nombre);

        var lista = await query.Select(c => new
        {
            c.Id,
            c.Nombre,
            c.Descripcion,
            c.Icono,
            c.Color,
            c.EsIngreso,
            c.EsGlobal,
            c.HogarId
        }).ToListAsync(ct);

        return Ok(lista);
    }
}
