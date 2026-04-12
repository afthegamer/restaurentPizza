using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using restaurent_pizza.Models;

namespace restaurent_pizza.Data.Configurations;

// 🟡 EF Core — configuration de la table "Users"
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(u => u.LastName).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Role).IsRequired();

        // 🟡 EF Core — index unique sur l'email (pas 2 comptes avec le même email)
        builder.HasIndex(u => u.Email).IsUnique();

        // 🟡 EF Core 10 — Named Query Filter soft delete
        builder.HasQueryFilter("SoftDelete", u => u.DeletedOn == null);
    }
}
