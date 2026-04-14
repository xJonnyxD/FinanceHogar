using FinanceHogar.Application.DTOs.ServiciosBasicos;
using FluentValidation;

namespace FinanceHogar.Application.Validators.ServiciosBasicos;

public class CreateServicioBasicoRequestValidator : AbstractValidator<CreateServicioBasicoRequest>
{
    public CreateServicioBasicoRequestValidator()
    {
        RuleFor(x => x.HogarId)
            .NotEmpty().WithMessage("El ID del hogar es obligatorio.");

        RuleFor(x => x.TipoServicio)
            .InclusiveBetween(1, 10).WithMessage("El tipo de servicio debe ser un valor válido (1-10).");

        RuleFor(x => x.NombreProveedor)
            .NotEmpty().WithMessage("El nombre del proveedor es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres.");

        RuleFor(x => x.MontoPromedio)
            .GreaterThan(0).WithMessage("El monto promedio debe ser mayor a cero.");

        RuleFor(x => x.FechaVencimiento)
            .NotEmpty().WithMessage("La fecha de vencimiento es obligatoria.")
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("La fecha de vencimiento no puede ser pasada.");

        RuleFor(x => x.DiasAnticipacionNotificacion)
            .InclusiveBetween(1, 30).WithMessage("Los días de anticipación deben estar entre 1 y 30.");
    }
}
