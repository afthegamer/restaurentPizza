using MediatR;
using Microsoft.EntityFrameworkCore;
using restaurent_pizza.Data;
using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Features.Order.Queries.GetAll;

// 🟡 MediatR — Handler Admin qui récupère toutes les commandes
public class GetAllOrdersQueryHandler(PizzaDbContext context)
    : IRequestHandler<GetAllOrdersQuery, List<OrderResult>>
{
    public async Task<List<OrderResult>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await context.Orders
            .Include(o => o.Items)                       // 🟡 EF Core — charge les lignes de chaque commande
            .OrderByDescending(o => o.CreatedOn)         // 🔵 Les plus récentes en premier
            .ToListAsync(cancellationToken);

        return orders.Select(o => new OrderResult(o)).ToList();
    }
}
