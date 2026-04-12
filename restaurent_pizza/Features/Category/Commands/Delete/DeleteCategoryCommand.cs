namespace restaurent_pizza.Features.Category.Commands.Delete;

public record DeleteCategoryCommand(Guid Id) : ICommand;