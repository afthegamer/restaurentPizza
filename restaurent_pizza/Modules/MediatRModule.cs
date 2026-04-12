using FluentValidation;
using restaurent_pizza.Features.Behaviors;

namespace restaurent_pizza.Modules;

// 🔵 C# pur — module d'enregistrement MediatR (regroupe la config MediatR + FluentValidation)
public static class MediatRModule
{
    public static IServiceCollection AddMediatRModule(this IServiceCollection services)
    {
        // 🟡 MediatR — scanne l'assembly pour trouver tous les Handlers automatiquement
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<Program>();          // 🟡 MediatR — scanne tout le projet
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));            // 🟡 MediatR — branche le pipeline de validation
        });

        // 🟡 FluentValidation — scanne l'assembly pour trouver tous les validateurs (AbstractValidator<T>)
        services.AddValidatorsFromAssemblyContaining<Program>();

        return services;
    }
}