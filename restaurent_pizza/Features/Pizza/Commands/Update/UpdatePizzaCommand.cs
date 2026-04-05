namespace restaurent_pizza.Features.Pizza.Commands.Update;

// 🟡 MediatR — Command de modification (pas de retour → ICommand sans <T>)
// L'Id vient de la route, les données du body → les deux sont dans la Command
public record UpdatePizzaCommand(Guid Id, string Name, string Description, decimal Price, bool IsAvailable) : ICommand;