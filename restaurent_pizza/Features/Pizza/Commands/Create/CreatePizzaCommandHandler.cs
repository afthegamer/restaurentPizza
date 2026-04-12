using MediatR;
using restaurent_pizza.Data;
using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Features.Pizza.Commands.Create;

// 🟡 MediatR — Handler de création (comme CreateCarouselInfoCommandHandler au travail)
public class CreatePizzaCommandHandler(PizzaDbContext context)
    : IRequestHandler<CreatePizzaCommand, PizzaResult>
{
    public async Task<PizzaResult> Handle(CreatePizzaCommand request, CancellationToken cancellationToken)
    {
        // 🔵 Factory Method — l'entité contrôle sa propre création
        var pizza = Models.Pizza.Create(request.Name, request.Description, request.Price, request.CategoryId);

        context.Pizzas.Add(pizza);
        await context.SaveChangesAsync(cancellationToken);

        return new PizzaResult(pizza);
    }
}