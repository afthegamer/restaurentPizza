using MediatR;
using Microsoft.EntityFrameworkCore;
using restaurent_pizza.Data;
using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Features.Pizza.Queries.GetAll;

// 🟡 MediatR — le Handler = la logique métier (comme GetAllCarouselInfoQueryHandler au travail)
// MediatR reçoit la Query et trouve automatiquement ce Handler grâce au typage générique
public class GetAllPizzasQueryHandler(PizzaDbContext context) 
    : IRequestHandler<GetAllPizzasQuery, List<PizzaResult>>   // 🟡 MediatR — IRequestHandler<TRequest, TResponse>
{
    public async Task<List<PizzaResult>> Handle(GetAllPizzasQuery request, CancellationToken cancellationToken)
    {
        // 🟡 EF Core — même code que dans PizzaService, mais isolé dans son propre Handler
        return await context.Pizzas
            .Select(p => new PizzaResult(p))
            .ToListAsync(cancellationToken);
    }
}