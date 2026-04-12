using MediatR;
using Microsoft.EntityFrameworkCore;
using restaurent_pizza.Data;
using restaurent_pizza.Exceptions;
using restaurent_pizza.Models;
using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Features.Order.Commands.Create;

// 🟡 MediatR — Handler qui exécute la logique de création de commande
public class CreateOrderCommandHandler(PizzaDbContext context)
    : IRequestHandler<CreateOrderCommand, OrderResult>
{
    public async Task<OrderResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // 🔵 Récupère toutes les pizzas demandées en un seul appel BDD (performance)
        var pizzaIds = request.Items.Select(i => i.PizzaId).ToList();
        var pizzas = await context.Pizzas
            .Where(p => pizzaIds.Contains(p.Id) && p.IsAvailable)
            .ToDictionaryAsync(p => p.Id, cancellationToken);    // 🟡 EF Core — Dictionary pour lookup rapide par Id

        // 🔵 Vérifie que TOUTES les pizzas existent et sont disponibles
        foreach (var item in request.Items)
        {
            if (!pizzas.ContainsKey(item.PizzaId))
                throw new EntityNotFoundException("Pizza", item.PizzaId);
        }

        // 🔵 Crée les OrderItems avec snapshot (prix + nom figés)
        var orderItems = request.Items
            .Select(item => OrderItem.Create(pizzas[item.PizzaId], item.Quantity))
            .ToList();

        // 🔵 Factory Method — crée la commande avec calcul du total
        var order = Models.Order.Create(request.UserId, orderItems);

        context.Orders.Add(order);
        await context.SaveChangesAsync(cancellationToken);

        return new OrderResult(order);
    }
}
