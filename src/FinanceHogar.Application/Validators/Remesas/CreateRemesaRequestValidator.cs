using FinanceHogar.Application.DTOs.Remesas;
using FluentValidation;

namespace FinanceHogar.Application.Validators.Remesas;

public class CreateRemesaRequestValidator : AbstractValidator<CreateRemesaRequest>
{
    private static readonly string[] MonedasValidas = ["USD", "BTC"];

    public CreateRemesaRequestValidator()
    {
        RuleFor(x => x.HogarId)
            .NotEmpty().WithMessage("El ID del hogar es obligatorio.");

        RuleFor(x => x.CategoriaId)
            .NotEmpty().WithMessage("La categoría es obligatoria.");

        RuleFor(x => x.Monto)
            .GreaterThan(0).WithMessage("El monto de la remesa debe ser mayor a cero.");

        RuleFor(x => x.Moneda)
            .Must(m => MonedasValidas.Contains(m.ToUpper()))
            .WithMessage("La moneda debe ser USD o BTC.");

        RuleFor(x => x.MontoEnUSD)
            .NotNull().When(x => x.Moneda?.ToUpper() == "BTC")
            .WithMessage("Debe indicar el equivalente en USD cuando la moneda es BTC.")
            .GreaterThan(0).When(x => x.MontoEnUSD.HasValue)
            .WithMessage("El monto en USD debe ser mayor a cero.");

        RuleFor(x => x.PaisOrigen)
            .NotEmpty().WithMessage("El país de origen es obligatorio.")
            .MaximumLength(100);

        RuleFor(x => x.Empresa)
            .NotEmpty().WithMessage("La empresa remesadora es obligatoria.")
            .MaximumLength(100).WithMessage("El nombre de la empresa no puede exceder 100 caracteres.");

        RuleFor(x => x.FechaRecepcion)
            .NotEmpty().WithMessage("La fecha de recepción es obligatoria.")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today.AddDays(1)))
            .WithMessage("La fecha de recepción no puede ser futura.");

        RuleFor(x => x.NumeroConfirmacion)
            .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.NumeroConfirmacion));
    }
}
