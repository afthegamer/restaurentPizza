using MediatR;
using Microsoft.EntityFrameworkCore;
using restaurent_pizza.Data;
using restaurent_pizza.Exceptions;

namespace restaurent_pizza.Features.Pizza.Commands.Update;

// 🟡 MediatR — Handler de modification
public class UpdatePizzaCommandHandler(PizzaDbContext context)
    : IRequestHandler<UpdatePizzaCommand>                      // 🟡 MediatR — pas de TResponse = void (Unit en interne)
{
    public async Task Handle(UpdatePizzaCommand request, CancellationToken cancellationToken)
    {
        var pizza = await context.Pizzas
                        .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
                    ?? throw new EntityNotFoundException("Pizza", request.Id);

        pizza.Name = request.Name;
        pizza.Description = request.Description;
        pizza.Price = request.Price;
        pizza.IsAvailable = request.IsAvailable;

        await context.SaveChangesAsync(cancellationToken);
    }
}