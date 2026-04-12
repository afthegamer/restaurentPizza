using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Features.Pizza.Commands.Create;

// 🟡 MediatR — Command = opération d'écriture (comme CreateCarouselInfoCommand au travail)
// Contient les données nécessaires à la création
// Retourne PizzaResult car le Controller a besoin de l'Id pour le header Location (CreatedAtAction)
public record CreatePizzaCommand(string Name, string Description, decimal Price, Guid CategoryId) : ICommand<PizzaResult>;