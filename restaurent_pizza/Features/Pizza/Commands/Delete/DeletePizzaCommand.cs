namespace restaurent_pizza.Features.Pizza.Commands.Delete;

// 🟡 MediatR — Command de suppression (soft delete)
public record DeletePizzaCommand(Guid Id) : ICommand;