using MediatR;
using Microsoft.EntityFrameworkCore;
using restaurent_pizza.Data;
using restaurent_pizza.Exceptions;
using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Features.Pizza.Queries.GetById;

// 🟡 MediatR — Handler pour récupérer une pizza par Id
public class GetPizzaByIdQueryHandler(PizzaDbContext context)
    : IRequestHandler<GetPizzaByIdQuery, PizzaResult>
{
    public async Task<PizzaResult> Handle(GetPizzaByIdQuery request, CancellationToken cancellationToken)
    {
        var pizza = await context.Pizzas
                        .Include(p => p.Category)
                        .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken)
                    ?? throw new EntityNotFoundException("Pizza", request.Id);  // 🔵 Même pattern que dans le Service

        return new PizzaResult(pizza);
    }
}