using MediatR;
using Microsoft.EntityFrameworkCore;
using restaurent_pizza.Data;
using restaurent_pizza.Exceptions;
using restaurent_pizza.Models;

namespace restaurent_pizza.Features.Order.Commands.UpdateStatus;

// 🟡 MediatR — Handler Admin qui change le statut d'une commande
public class UpdateOrderStatusCommandHandler(PizzaDbContext context)
    : IRequestHandler<UpdateOrderStatusCommand>
{
    public async Task Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await context.Orders
            .FirstOrDefaultAsync(o => o.Id == request.Id, cancellationToken)
            ?? throw new EntityNotFoundException("Order", request.Id);

        // 🔵 Parse le string en enum (avec validation)
        if (!Enum.TryParse<OrderStatus>(request.Status, ignoreCase: true, out var newStatus))
            throw new InvalidOperationException($"Statut invalide : '{request.Status}'. Valeurs possibles : {string.Join(", ", Enum.GetNames<OrderStatus>())}");

        // 🔵 Méthode métier de l'entité (l'entité encapsule ses propres transitions d'état)
        order.UpdateStatus(newStatus);

        await context.SaveChangesAsync(cancellationToken);
    }
}
