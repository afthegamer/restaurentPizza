namespace restaurent_pizza.Models;

// 🔵 C# pur — classe abstraite (on ne peut pas créer un "new BaseEntity()" directement)
// Comme Entity au travail : toutes les entités en héritent
public abstract class BaseEntity
{
    public Guid Id { get; set; }                    // 🔵 Guid — identifiant unique universel (comme au travail)
    public DateTimeOffset CreatedOn { get; set; }   // 🔵 DateTimeOffset — inclut le fuseau horaire
    public DateTimeOffset? UpdatedOn { get; set; }  // 🔵 Nullable (?) — null tant que pas modifié
    public DateTimeOffset? DeletedOn { get; set; }  // 🔵 Nullable — null = pas supprimé (Soft Delete)
}
