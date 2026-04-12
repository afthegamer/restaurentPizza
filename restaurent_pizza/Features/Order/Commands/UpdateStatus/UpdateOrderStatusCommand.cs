using MediatR;

namespace restaurent_pizza.Features.Order.Commands.UpdateStatus;

// 🟡 MediatR — Command Admin : changer le statut d'une commande
public record UpdateOrderStatusCommand(
    Guid Id,                             // 🔵 L'Id de la commande à modifier
    string Status                        // 🔵 Le nouveau statut ("Preparing", "Ready", "Delivered", "Cancelled")
) : IRequest;                            // 🟡 MediatR — IRequest sans <T> = pas de retour (void)
