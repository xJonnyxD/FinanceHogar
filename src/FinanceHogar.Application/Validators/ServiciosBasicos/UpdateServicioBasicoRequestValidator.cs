using FinanceHogar.Application.DTOs.ServiciosBasicos;
using FluentValidation;

namespace FinanceHogar.Application.Validators.ServiciosBasicos;

public class UpdateServicioBasicoRequestValidator : AbstractValidator<UpdateServicioBasicoRequest>
{
    public UpdateServicioBasicoRequestValidator()
    {
        RuleFor(x => x.NombreProveedor)
            .NotEmpty().WithMessage("El nombre del proveedor es obligatorio.")
            .MaximumLength(100);

        RuleFor(x => x.MontoPromedio)
            .GreaterThan(0).WithMessage("El monto promedio debe ser mayor a cero.");

        RuleFor(x => x.FechaVencimiento)
            .NotEmpty().WithMessage("La fecha de vencimiento es obligatoria.");

        RuleFor(x => x.DiasAnticipacionNotificacion)
            .InclusiveBetween(1, 30).WithMessage("Los días de anticipación deben estar entre 1 y 30.");
    }
}
