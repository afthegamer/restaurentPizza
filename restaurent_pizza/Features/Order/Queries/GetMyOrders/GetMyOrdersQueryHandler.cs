using MediatR;
using Microsoft.EntityFrameworkCore;
using restaurent_pizza.Data;
using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Features.Order.Queries.GetMyOrders;

// 🟡 MediatR — Handler qui récupère les commandes du client connecté
public class GetMyOrdersQueryHandler(PizzaDbContext context)
    : IRequestHandler<GetMyOrdersQuery, List<OrderResult>>
{
    public async Task<List<OrderResult>> Handle(GetMyOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await context.Orders
            .Where(o => o.UserId == request.UserId)     // 🔵 Filtre par user connecté (sécurité !)
            .Include(o => o.Items)                       // 🟡 EF Core — charge les lignes de commande
            .OrderByDescending(o => o.CreatedOn)         // 🔵 Les plus récentes en premier
            .ToListAsync(cancellationToken);

        return orders.Select(o => new OrderResult(o)).ToList();
    }
}
