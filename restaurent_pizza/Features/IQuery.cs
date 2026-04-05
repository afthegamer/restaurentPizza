using MediatR;

namespace restaurent_pizza.Features;

// 🟡 MediatR — interface marqueur pour les Queries (lecture)
// Une Query retourne TOUJOURS quelque chose (sinon c'est pas une query !)
public interface IQuery<out TResponse> : IRequest<TResponse>;