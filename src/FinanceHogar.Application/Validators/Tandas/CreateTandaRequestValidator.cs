using FinanceHogar.Application.DTOs.Tandas;
using FluentValidation;

namespace FinanceHogar.Application.Validators.Tandas;

public class CreateTandaRequestValidator : AbstractValidator<CreateTandaRequest>
{
    public CreateTandaRequestValidator()
    {
        RuleFor(x => x.HogarId)
            .NotEmpty().WithMessage("El ID del hogar es obligatorio.");

        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la tanda es obligatorio.")
            .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres.");

        RuleFor(x => x.CuotaMensual)
            .GreaterThan(0).WithMessage("La cuota mensual debe ser mayor a cero.");

        RuleFor(x => x.TotalParticipantes)
            .GreaterThanOrEqualTo(2).WithMessage("Una tanda necesita al menos 2 participantes.")
            .LessThanOrEqualTo(50).WithMessage("Una tanda no puede tener más de 50 participantes.");

        RuleFor(x => x.FechaInicio)
            .NotEmpty().WithMessage("La fecha de inicio es obligatoria.");
    }
}
