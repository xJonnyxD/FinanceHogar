using FinanceHogar.Application.DTOs.Presupuestos;
using FluentValidation;

namespace FinanceHogar.Application.Validators.Presupuestos;

public class UpdatePresupuestoRequestValidator : AbstractValidator<UpdatePresupuestoRequest>
{
    public UpdatePresupuestoRequestValidator()
    {
        RuleFor(x => x.MontoLimite)
            .GreaterThan(0).WithMessage("El monto límite debe ser mayor a cero.");
    }
}
