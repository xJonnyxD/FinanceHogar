using FinanceHogar.Application.DTOs.Ingresos;
using FinanceHogar.Domain.Enums;
using FluentValidation;

namespace FinanceHogar.Application.Validators.Ingresos;

public class UpdateIngresoRequestValidator : AbstractValidator<UpdateIngresoRequest>
{
    public UpdateIngresoRequestValidator()
    {
        RuleFor(x => x.CategoriaId)
            .NotEmpty().WithMessage("La categoría es obligatoria.");

        RuleFor(x => x.Monto)
            .GreaterThan(0).WithMessage("El monto debe ser mayor a cero.");

        RuleFor(x => x.FechaIngreso)
            .NotEmpty().WithMessage("La fecha del ingreso es obligatoria.");

        RuleFor(x => x.Frecuencia)
            .NotNull().When(x => x.EsRecurrente)
            .WithMessage("Debe indicar la frecuencia cuando el ingreso es recurrente.");
    }
}
