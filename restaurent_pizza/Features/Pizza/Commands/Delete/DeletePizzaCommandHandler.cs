using MediatR;
using Microsoft.EntityFrameworkCore;
using restaurent_pizza.Data;
using restaurent_pizza.Exceptions;

namespace restaurent_pizza.Features.Pizza.Commands.Delete;

// 🟡 MediatR — Handler de suppression (soft delete via pizza.Delete())
public class DeletePizzaCommandHandler(PizzaDbContext context)
    : IRequestHandler<DeletePizzaCommand>
{
    public async Task Handle(DeletePizzaCommand request, CancellationToken cancellationToken)
    {
        var pizza = await context.Pizzas
                        .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
                    ?? throw new EntityNotFoundException("Pizza", request.Id);

        pizza.Delete();                                        // 🔵 Soft Delete — met DeletedOn = UtcNow
        await context.SaveChangesAsync(cancellationToken);
    }
}