using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using restaurent_pizza.Models;

namespace restaurent_pizza.Data.Configurations;

// 🟡 EF Core — configuration de la table "Categories" en BDD
public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Property(c => c.Description).HasMaxLength(500);

        // 🟡 EF Core — relation 1:N côté parent (comme dans les configurations au travail)
        builder.HasMany(c => c.Pizzas)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);            // 🟡 Restrict = pas de cascade (obligatoire avec soft delete)

        // 🟡 EF Core 10 — Named Query Filter pour le soft delete
        builder.HasQueryFilter("SoftDelete", c => c.DeletedOn == null);
    }
}