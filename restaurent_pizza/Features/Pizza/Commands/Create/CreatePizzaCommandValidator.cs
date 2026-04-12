using FluentValidation;

namespace restaurent_pizza.Features.Pizza.Commands.Create;

// 🟡 FluentValidation — validateur pour la Command (comme CreateCarouselInfoCommandValidator au travail)
// Placé dans le même dossier que la Command → feature folder = tout est regroupé
public class CreatePizzaCommandValidator : AbstractValidator<CreatePizzaCommand>
{
    public CreatePizzaCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Le nom de la pizza est requis.")
            .MaximumLength(100).WithMessage("Le nom ne peut pas dépasser 100 caractères.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("La description ne peut pas dépasser 500 caractères.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Le prix doit être supérieur à 0.")
            .LessThan(1000).WithMessage("Le prix ne peut pas dépasser 999.99€.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("La catégorie est requise.");  // 🟡 NotEmpty sur Guid = != Guid.Empty
    }
}