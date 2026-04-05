using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Features.Pizza.Queries.GetAll;

// 🟡 MediatR — la Query = le message envoyé par le Controller
// C'est un record vide : "donne-moi toutes les pizzas", pas de paramètre
public record GetAllPizzasQuery : IQuery<List<PizzaResult>>;