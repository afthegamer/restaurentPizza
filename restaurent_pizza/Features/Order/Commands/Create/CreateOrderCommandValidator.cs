using FluentValidation;

namespace restaurent_pizza.Features.Order.Commands.Create;

// 🟡 FluentValidation — valide la commande avant qu'elle arrive au Handler
public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("L'identifiant du client est requis.");

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("La commande doit contenir au moins une pizza.");

        // 🟡 FluentValidation — RuleForEach = applique les règles à CHAQUE élément de la liste
        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.PizzaId)
                .NotEmpty().WithMessage("L'identifiant de la pizza est requis.");

            item.RuleFor(i => i.Quantity)
                .GreaterThan(0).WithMessage("La quantité doit être supérieure à 0.")
                .LessThanOrEqualTo(20).WithMessage("La quantité ne peut pas dépasser 20 par pizza.");
        });
    }
}
