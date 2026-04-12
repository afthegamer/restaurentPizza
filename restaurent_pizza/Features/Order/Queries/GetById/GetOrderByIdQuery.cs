using MediatR;
using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Features.Order.Queries.GetById;

// 🟡 MediatR — Query pour récupérer une commande par son Id
// UserId sert à vérifier que le client ne consulte que SES commandes (sauf Admin)
public record GetOrderByIdQuery(
    Guid Id,                             // 🔵 L'Id de la commande demandée
    Guid UserId,                         // 🔵 L'Id du client connecté (depuis le token JWT)
    bool IsAdmin                         // 🔵 true = Admin (peut voir toutes les commandes)
) : IRequest<OrderResult>;
