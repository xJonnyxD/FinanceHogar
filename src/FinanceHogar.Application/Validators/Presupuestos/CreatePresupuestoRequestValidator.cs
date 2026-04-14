using FinanceHogar.Application.DTOs.Presupuestos;
using FluentValidation;

namespace FinanceHogar.Application.Validators.Presupuestos;

public class CreatePresupuestoRequestValidator : AbstractValidator<CreatePresupuestoRequest>
{
    public CreatePresupuestoRequestValidator()
    {
        RuleFor(x => x.HogarId)
            .NotEmpty().WithMessage("El ID del hogar es obligatorio.");

        RuleFor(x => x.CategoriaId)
            .NotEmpty().WithMessage("La categoría es obligatoria.");

        RuleFor(x => x.Anio)
            .InclusiveBetween(2020, 2100).WithMessage("El año debe estar entre 2020 y 2100.");

        RuleFor(x => x.Mes)
            .InclusiveBetween(1, 12).WithMessage("El mes debe estar entre 1 y 12.");

        RuleFor(x => x.MontoLimite)
            .GreaterThan(0).WithMessage("El monto límite debe ser mayor a cero.");
    }
}
