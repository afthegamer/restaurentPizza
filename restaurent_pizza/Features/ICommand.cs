using MediatR;

namespace restaurent_pizza.Features;

// 🟡 MediatR — interface marqueur pour les Commands (écriture)
// ICommand = pas de retour (void) → pour Update, Delete
// ICommand<TResponse> = avec retour → pour Create (retourne le Result)
public interface ICommand : IRequest;                          // 🟡 MediatR — IRequest sans <T> = void
public interface ICommand<out TResponse> : IRequest<TResponse>;  // 🟡 MediatR — IRequest<T> = retourne T