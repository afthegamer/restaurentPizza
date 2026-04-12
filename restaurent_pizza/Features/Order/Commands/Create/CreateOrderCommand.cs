using MediatR;
using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Features.Order.Commands.Create;

// 🟡 MediatR — Command = intention d'écriture ("je veux passer une commande")
// Le UserId vient du token JWT (injecté par le Controller, pas par le client)
public record CreateOrderCommand(
    Guid UserId,                         // 🔵 Rempli par le Controller depuis le token JWT (pas envoyé par le client !)
    List<CreateOrderItemDto> Items       // 🔵 La liste des pizzas commandées (envoyée par le client)
) : IRequest<OrderResult>;              // 🟡 MediatR — retourne un OrderResult
