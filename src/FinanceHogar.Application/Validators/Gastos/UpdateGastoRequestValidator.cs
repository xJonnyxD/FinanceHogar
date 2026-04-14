using FinanceHogar.Application.DTOs.Gastos;
using FinanceHogar.Domain.Enums;
using FluentValidation;

namespace FinanceHogar.Application.Validators.Gastos;

public class UpdateGastoRequestValidator : AbstractValidator<UpdateGastoRequest>
{
    public UpdateGastoRequestValidator()
    {
        RuleFor(x => x.CategoriaId)
            .NotEmpty().WithMessage("La categoría es obligatoria.");

        RuleFor(x => x.Monto)
            .GreaterThan(0).WithMessage("El monto debe ser mayor a cero.");

        RuleFor(x => x.FechaGasto)
            .NotEmpty().WithMessage("La fecha del gasto es obligatoria.");

        RuleFor(x => x.Frecuencia)
            .NotNull().When(x => x.EsRecurrente)
            .WithMessage("Debe indicar la frecuencia cuando el gasto es recurrente.");
    }
}
