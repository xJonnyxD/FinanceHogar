using FinanceHogar.Application.DTOs.Hogares;
using FluentValidation;

namespace FinanceHogar.Application.Validators.Hogares;

public class CreateHogarRequestValidator : AbstractValidator<CreateHogarRequest>
{
    private static readonly string[] MonedasValidas = ["USD", "BTC"];

    public CreateHogarRequestValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del hogar es obligatorio.")
            .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres.");

        RuleFor(x => x.PresupuestoMensualTotal)
            .GreaterThanOrEqualTo(0).WithMessage("El presupuesto mensual no puede ser negativo.");

        RuleFor(x => x.MonedaPrincipal)
            .Must(m => MonedasValidas.Contains(m.ToUpper()))
            .WithMessage("La moneda principal debe ser USD o BTC.");

        RuleFor(x => x.Departamento)
            .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Departamento));

        RuleFor(x => x.Municipio)
            .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Municipio));
    }
}
