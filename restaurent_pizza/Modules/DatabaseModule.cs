namespace restaurent_pizza.Modules;

// 🔵 C# pur — extension method pour regrouper les registrations DI (comme DatabaseModule au travail)
// Chaque module regroupe les services liés à un domaine
// Au travail : DatabaseModule, RestServicesModule, JobModule, KafkaModule
public static class DatabaseModule
{
    public static IServiceCollection AddDatabaseModule(this IServiceCollection services)
    {
        // Les Handlers MediatR sont enregistrés dans MediatRModule
        // On ajoutera ici les services non-MediatR (ICategoryService, IOrderService...)
        return services;
    }
}