using FluentValidation;
using MediatR;

namespace restaurent_pizza.Features.Behaviors;

// 🟡 MediatR — Pipeline Behavior = middleware qui s'exécute avant chaque Handler
// Intercepte CHAQUE requête MediatR AVANT le Handler
// Si des validateurs existent pour ce type de requête → les exécute
// Si la validation échoue → lance ValidationException → GlobalExceptionFilter → 400
public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>                   // 🟡 MediatR — interface pipeline
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,                // 🟡 MediatR — next = le Handler (ou le behavior suivant)
        CancellationToken cancellationToken)
    {
        // 🔵 S'il n'y a aucun validateur pour cette requête → on passe directement au Handler
        if (!validators.Any())
            return await next(cancellationToken);

        // 🟡 FluentValidation — exécute TOUS les validateurs en parallèle
        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        // 🔵 C# pur — agrège toutes les erreurs de tous les validateurs
        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        // 🟡 FluentValidation — si des erreurs → lance l'exception (interceptée par GlobalExceptionFilter)
        if (failures.Count != 0)
            throw new ValidationException(failures);

        // ✅ Validation OK → passe au Handler
        return await next(cancellationToken);
    }
}