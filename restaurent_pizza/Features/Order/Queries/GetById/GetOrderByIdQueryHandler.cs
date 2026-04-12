using MediatR;
using Microsoft.EntityFrameworkCore;
using restaurent_pizza.Data;
using restaurent_pizza.Exceptions;
using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Features.Order.Queries.GetById;

// 🟡 MediatR — Handler qui récupère une commande par Id
public class GetOrderByIdQueryHandler(PizzaDbContext context)
    : IRequestHandler<GetOrderByIdQuery, OrderResult>
{
    public async Task<OrderResult> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await context.Orders
            .Include(o => o.Items)                       // 🟡 EF Core — charge les lignes
            .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Order", request.Id);

        // 🔵 Sécurité : un Client ne peut voir que SES commandes, un Admin peut tout voir
        if (!request.IsAdmin && order.UserId != request.UserId)
            throw new EntityNotFoundException("Order", request.Id);  // 🔵 On renvoie 404 (pas 403) pour ne pas révéler que la commande existe

        return new OrderResult(order);
    }
}
