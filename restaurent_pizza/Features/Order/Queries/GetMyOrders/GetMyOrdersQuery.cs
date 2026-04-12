using MediatR;
using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Features.Order.Queries.GetMyOrders;

// 🟡 MediatR — Query = intention de lecture ("donne-moi MES commandes")
// Le UserId vient du token JWT (le client ne voit que SES commandes, pas celles des autres)
public record GetMyOrdersQuery(
    Guid UserId                          // 🔵 Rempli par le Controller depuis le token JWT
) : IRequest<List<OrderResult>>;         // 🟡 MediatR — retourne une liste de commandes
