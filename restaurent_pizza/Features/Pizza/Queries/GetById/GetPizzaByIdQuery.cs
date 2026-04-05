using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Features.Pizza.Queries.GetById;

// 🟡 MediatR — Query avec paramètre : l'Id de la pizza à chercher
// record avec un paramètre = le message porte les données nécessaires au Handler
public record GetPizzaByIdQuery(Guid Id) : IQuery<PizzaResult>;