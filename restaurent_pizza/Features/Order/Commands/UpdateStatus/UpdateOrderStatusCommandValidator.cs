using FluentValidation;

namespace restaurent_pizza.Features.Order.Commands.UpdateStatus;

// 🟡 FluentValidation — valide le changement de statut
public class UpdateOrderStatusCommandValidator : AbstractValidator<UpdateOrderStatusCommand>
{
    public UpdateOrderStatusCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("L'identifiant de la commande est requis.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Le nouveau statut est requis.");
    }
}
