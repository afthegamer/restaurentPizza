using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Features.Category.Commands.Create;

public record CreateCategoryCommand(string Name, string Description) : ICommand<CategoryResult>;