using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using restaurent_pizza.Models;

namespace restaurent_pizza.Data.Configurations;

// 🟡 EF Core — configuration de la table "Pizzas" en BDD
// Pattern IEntityTypeConfiguration : une config par entité, séparée du DbContext
// Trouvée automatiquement par ApplyConfigurationsFromAssembly()
public class PizzaConfiguration : IEntityTypeConfiguration<Pizza>
{
    public void Configure(EntityTypeBuilder<Pizza> builder)
    {
        builder.ToTable("Pizzas");                                    // 🟡 Nom de la table en BDD
        builder.HasKey(p => p.Id);                                    // 🟡 Clé primaire = Id (Guid)

        builder.Property(p => p.Name).IsRequired().HasMaxLength(100); // 🟡 NOT NULL, max 100 chars
        builder.Property(p => p.Description).HasMaxLength(500);       // 🟡 max 500 chars (nullable OK)
        builder.Property(p => p.Price).HasPrecision(10, 2);           // 🟡 decimal(10,2) = 99999999.99 max
        builder.Property(p => p.CostPrice).HasPrecision(10, 2);      // 🟡 même précision pour le coût*
        // 🟡 EF Core — FK vers Category (relation 1:N côté enfant)
        builder.Property(p => p.CategoryId).IsRequired();

        // 🟡 EF Core 10 — Named Query Filter pour le soft delete !
        // Nouveau dans EF Core 10 : on peut nommer le filtre et le désactiver sélectivement
        builder.HasQueryFilter("SoftDelete", p => p.DeletedOn == null);
    }
}