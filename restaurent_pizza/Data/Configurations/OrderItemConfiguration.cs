using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using restaurent_pizza.Models;

namespace restaurent_pizza.Data.Configurations;

// 🟡 EF Core — configuration de la table "OrderItems"
public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("OrderItems");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Quantity).IsRequired();

        // 🔵 SNAPSHOT — ces valeurs sont figées au moment de la commande
        builder.Property(i => i.UnitPrice)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(i => i.PizzaName)
            .IsRequired()
            .HasMaxLength(100);

        // 🟡 EF Core — LineTotal est une propriété calculée C# (pas stockée en BDD)
        builder.Ignore(i => i.LineTotal);

        // 🟡 EF Core — relation Pizza (1) → OrderItems (N)
        // OnDelete Restrict = interdit de supprimer une pizza référencée dans une commande
        builder.HasOne(i => i.Pizza)
            .WithMany()
            .HasForeignKey(i => i.PizzaId)
            .OnDelete(DeleteBehavior.Restrict);

        // 🟡 EF Core — Named Query Filter (soft delete automatique)
        builder.HasQueryFilter("SoftDelete", i => i.DeletedOn == null);
    }
}
