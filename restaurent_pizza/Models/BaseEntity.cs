namespace restaurent_pizza.Models;

// 🔵 C# pur — classe abstraite (on ne peut pas créer un "new BaseEntity()" directement)
// Classe abstraite : toutes les entités en héritent (Id, timestamps, soft delete)
public abstract class BaseEntity
{
    public Guid Id { get; set; }                    // 🔵 Guid — identifiant unique universel (pas d'auto-increment, fonctionne en distribué)
    public DateTimeOffset CreatedOn { get; set; }   // 🔵 DateTimeOffset — inclut le fuseau horaire
    public DateTimeOffset? UpdatedOn { get; set; }  // 🔵 Nullable (?) — null tant que pas modifié
    public DateTimeOffset? DeletedOn { get; set; }  // 🔵 Nullable — null = pas supprimé (Soft Delete)

    // 🔵 C# pur — Soft Delete commun à toutes les entités
    // On ne supprime PAS de la BDD — on marque une date de suppression
    public void Delete()
    {
        if (DeletedOn == null)                     // 🔵 Seulement si pas déjà supprimé
            DeletedOn = DateTimeOffset.UtcNow;
    }
}
