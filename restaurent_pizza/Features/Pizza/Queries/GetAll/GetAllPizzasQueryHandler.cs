using MediatR;
using Microsoft.EntityFrameworkCore;
using restaurent_pizza.Data;
using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Features.Pizza.Queries.GetAll;

// 🟡 MediatR — le Handler = la logique métier (1 Query = 1 Handler = 1 responsabilité)
// MediatR reçoit la Query et trouve automatiquement ce Handler grâce au typage générique
public class GetAllPizzasQueryHandler(PizzaDbContext context) 
    : IRequestHandler<GetAllPizzasQuery, List<PizzaResult>>   // 🟡 MediatR — IRequestHandler<TRequest, TResponse>
{
    public async Task<List<PizzaResult>> Handle(GetAllPizzasQuery request, CancellationToken cancellationToken)
    {
        // 🟡 EF Core — Include charge la navigation Category (sans Include, Category serait null)
        return await context.Pizzas
            .Include(p => p.Category)
            .Select(p => new PizzaResult(p))
            .ToListAsync(cancellationToken);
    }
}