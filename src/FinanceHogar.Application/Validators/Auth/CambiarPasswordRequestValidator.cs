using FinanceHogar.Application.DTOs.Auth;
using FluentValidation;

namespace FinanceHogar.Application.Validators.Auth;

public class CambiarPasswordRequestValidator : AbstractValidator<CambiarPasswordRequest>
{
    public CambiarPasswordRequestValidator()
    {
        RuleFor(x => x.PasswordActual)
            .NotEmpty().WithMessage("La contraseña actual es obligatoria.");

        RuleFor(x => x.PasswordNueva)
            .NotEmpty().WithMessage("La contraseña nueva es obligatoria.")
            .MinimumLength(8).WithMessage("La contraseña nueva debe tener al menos 8 caracteres.")
            .Matches("[A-Z]").WithMessage("Debe contener al menos una letra mayúscula.")
            .Matches("[0-9]").WithMessage("Debe contener al menos un número.")
            .NotEqual(x => x.PasswordActual).WithMessage("La contraseña nueva debe ser diferente a la actual.");
    }
}
