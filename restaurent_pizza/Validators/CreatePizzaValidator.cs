using FluentValidation;
using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Validators;

// 🟡 FluentValidation — validateur pour la création (comme CreateCarouselInfoCommandValidator au travail)
// 1 validateur par DTO = séparation claire des règles
public class CreatePizzaValidator : AbstractValidator<CreatePizzaDto>  // 🟡 FluentValidation — hérite de AbstractValidator<T>
{
    public CreatePizzaValidator()
    {
        RuleFor(x => x.Name)                                          // 🟡 FluentValidation — règle sur une propriété
            .NotEmpty().WithMessage("Le nom de la pizza est requis.")  // 🟡 .NotEmpty() = ni null, ni "", ni whitespace
            .MaximumLength(100).WithMessage("Le nom ne peut pas dépasser 100 caractères.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("La description ne peut pas dépasser 500 caractères.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Le prix doit être supérieur à 0.")
            .LessThan(1000).WithMessage("Le prix ne peut pas dépasser 999.99€.");
    }
}