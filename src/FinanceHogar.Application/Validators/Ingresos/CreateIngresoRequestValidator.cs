using FinanceHogar.Application.DTOs.Ingresos;
using FinanceHogar.Domain.Enums;
using FluentValidation;

namespace FinanceHogar.Application.Validators.Ingresos;

public class CreateIngresoRequestValidator : AbstractValidator<CreateIngresoRequest>
{
    public CreateIngresoRequestValidator()
    {
        RuleFor(x => x.HogarId)
            .NotEmpty().WithMessage("El ID del hogar es obligatorio.");

        RuleFor(x => x.CategoriaId)
            .NotEmpty().WithMessage("La categoría es obligatoria.");

        RuleFor(x => x.Monto)
            .GreaterThan(0).WithMessage("El monto debe ser mayor a cero.");

        RuleFor(x => x.MontoEnUSD)
            .GreaterThan(0).When(x => x.Moneda == TipoMoneda.BTC && x.MontoEnUSD.HasValue)
            .WithMessage("El monto en USD debe ser mayor a cero cuando la moneda es BTC.");

        RuleFor(x => x.FechaIngreso)
            .NotEmpty().WithMessage("La fecha del ingreso es obligatoria.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today.AddDays(1)))
            .WithMessage("La fecha del ingreso no puede ser futura.");

        RuleFor(x => x.Frecuencia)
            .NotNull().When(x => x.EsRecurrente)
            .WithMessage("Debe indicar la frecuencia cuando el ingreso es recurrente.");

        RuleFor(x => x.Descripcion)
            .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres.")
            .When(x => !string.IsNullOrEmpty(x.Descripcion));
    }
}
