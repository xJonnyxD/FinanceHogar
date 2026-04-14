using FinanceHogar.Application.DTOs.Auth;
using FluentValidation;

namespace FinanceHogar.Application.Validators.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.NombreCompleto)
            .NotEmpty().WithMessage("El nombre completo es obligatorio.")
            .MaximumLength(200).WithMessage("El nombre no puede exceder 200 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
            .EmailAddress().WithMessage("El correo electrónico no tiene un formato válido.")
            .MaximumLength(200);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.")
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.")
            .Matches("[A-Z]").WithMessage("Debe contener al menos una letra mayúscula.")
            .Matches("[0-9]").WithMessage("Debe contener al menos un número.");

        RuleFor(x => x.NombreHogar)
            .NotEmpty().WithMessage("El nombre del hogar es obligatorio.")
            .MaximumLength(200);

        RuleFor(x => x.Telefono)
            .Matches(@"^\+?[\d\s\-]{7,15}$").When(x => !string.IsNullOrWhiteSpace(x.Telefono))
            .WithMessage("El teléfono no tiene un formato válido.");

        RuleFor(x => x.DUI)
            .Matches(@"^\d{8}-\d$").When(x => !string.IsNullOrWhiteSpace(x.DUI))
            .WithMessage("El DUI debe tener el formato 00000000-0 (El Salvador).");
    }
}
