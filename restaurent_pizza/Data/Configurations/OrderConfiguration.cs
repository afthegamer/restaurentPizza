using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using restaurent_pizza.Models;

namespace restaurent_pizza.Data.Configurations;

// 🟡 EF Core — configuration de la table "Orders" (relations, contraintes, conversions)
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(o => o.Id);

        // 🟡 EF Core — le statut est un enum C# stocké en STRING en BDD (lisible dans pgAdmin)
        builder.Property(o => o.Status)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(o => o.Total)
            .HasPrecision(10, 2);

        // 🟡 EF Core — relation User (1) → Orders (N)
        // OnDelete Restrict = interdit de supprimer un user qui a des commandes
        builder.HasOne(o => o.User)
            .WithMany()                        // Pas de navigation inverse sur User (pas besoin)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🟡 EF Core — relation Order (1) → OrderItems (N)
        // OnDelete Cascade = supprimer une commande supprime ses lignes (c'est un bloc)
        builder.HasMany(o => o.Items)
            .WithOne(i => i.Order)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // 🟡 EF Core — Named Query Filter (soft delete automatique)
        builder.HasQueryFilter("SoftDelete", o => o.DeletedOn == null);
    }
}
