using Microsoft.EntityFrameworkCore;
using restaurent_pizza.Models;

namespace restaurent_pizza.Data;

// 🟡 EF Core — Le DbContext est la porte d'entrée vers la base de données
// Dans un vrai projet, le DbContext peut avoir des dizaines de DbSet
public class PizzaDbContext : DbContext
{
    // 🟡 EF Core — constructeur qui reçoit les options de connexion (injectées par Aspire)
    public PizzaDbContext(DbContextOptions<PizzaDbContext> options)
        : base(options) { }

    // 🟡 EF Core — chaque DbSet = une table en BDD
    // Syntaxe => Set<T>() (plus propre que { get; set; } = null!)
    public DbSet<Pizza> Pizzas => Set<Pizza>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    // 🟡 EF Core — override SaveChangesAsync pour auto-timestamping
    // Pattern auto-timestamping : CreatedOn et UpdatedOn sont remplis automatiquement
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

    // 🟡 EF Core — scan automatique de toutes les IEntityTypeConfiguration de l'assembly
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PizzaDbContext).Assembly);
    }
}