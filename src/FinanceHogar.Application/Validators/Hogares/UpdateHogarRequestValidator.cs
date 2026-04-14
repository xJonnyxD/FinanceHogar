using FinanceHogar.Application.DTOs.Hogares;
using FluentValidation;

namespace FinanceHogar.Application.Validators.Hogares;

public class UpdateHogarRequestValidator : AbstractValidator<UpdateHogarRequest>
{
    public UpdateHogarRequestValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del hogar es obligatorio.")
            .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres.");

        RuleFor(x => x.PresupuestoMensualTotal)
            .GreaterThanOrEqualTo(0).WithMessage("El presupuesto mensual no puede ser negativo.");
    }
}
