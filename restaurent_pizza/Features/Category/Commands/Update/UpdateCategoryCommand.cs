namespace restaurent_pizza.Features.Category.Commands.Update;

public record UpdateCategoryCommand(Guid Id, string Name, string Description) : ICommand;