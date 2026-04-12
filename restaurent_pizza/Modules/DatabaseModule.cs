namespace restaurent_pizza.Modules;

// 🔵 C# pur — extension method pour regrouper les registrations DI (pattern Module)
// Chaque module regroupe les services liés à un domaine
// Pattern classique : un module par domaine (DatabaseModule, AuthModule, etc.)
public static class DatabaseModule
{
    public static IServiceCollection AddDatabaseModule(this IServiceCollection services)
    {
        // Les Handlers MediatR sont enregistrés dans MediatRModule
        // On ajoutera ici les services non-MediatR (ICategoryService, IOrderService...)
        return services;
    }
}