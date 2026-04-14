using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace FinanceHogar.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected Guid UsuarioActualId =>
        Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    protected Guid HogarActualId =>
        Guid.TryParse(User.FindFirstValue("HogarId"), out var id) ? id : Guid.Empty;

    protected bool EsAdministrador =>
        User.FindFirstValue("EsAdministrador") == "true";
}
