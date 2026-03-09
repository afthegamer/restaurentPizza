using Microsoft.EntityFrameworkCore;
using restaurent_pizza.Models;

namespace restaurent_pizza.Data;

// 🟡 EF Core — Le DbContext est la porte d'entrée vers la base de données
// Comme ApplicationDbContext au travail (qui a 40+ DbSet)
public class PizzaDbContext : DbContext
{
    // 🟡 EF Core — constructeur qui reçoit les options de connexion (injectées par Aspire)
    public PizzaDbContext(DbContextOptions<PizzaDbContext> options)
        : base(options) { }

    // 🟡 EF Core — chaque DbSet = une table en BDD
    // Syntaxe => Set<T>() comme au travail (pas { get; set; } = null!)
    public DbSet<Pizza> Pizzas => Set<Pizza>();

    // 🟡 EF Core — override SaveChangesAsync pour auto-timestamping
    // Comme OnBeforeSaving() au travail : CreatedOn et UpdatedOn sont remplis automatiquement
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
                entry.Entity.CreatedOn = DateTimeOffset.UtcNow;
            if (entry.State == EntityState.Modified)
                entry.Entity.UpdatedOn = DateTimeOffset.UtcNow;
        }
        return await base.SaveChangesAsync(cancellationToken);
    }

    // 🟡 EF Core — scan automatique des configurations (comme au travail)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PizzaDbContext).Assembly);
    }
}