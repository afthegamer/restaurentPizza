using MediatR;
using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Features.Order.Queries.GetAll;

// 🟡 MediatR — Query Admin : récupère TOUTES les commandes (pas filtrées par user)
public record GetAllOrdersQuery() : IRequest<List<OrderResult>>;
