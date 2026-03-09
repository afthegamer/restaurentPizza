using restaurent_pizza.Services;

namespace restaurent_pizza.Modules;

// 🔵 C# pur — extension method pour regrouper les registrations DI (comme DatabaseModule au travail)
// Chaque module regroupe les services liés à un domaine
// Au travail : DatabaseModule, RestServicesModule, JobModule, KafkaModule
public static class DatabaseModule
{
    public static IServiceCollection AddDatabaseModule(this IServiceCollection services)
    {
        services.AddScoped<IPizzaService, PizzaService>();  // 🔴 ASP.NET DI — Scoped = 1 instance par requête HTTP
        // On ajoutera d'autres services ici plus tard (ICategoryService, IOrderService...)
        return services;
    }
}