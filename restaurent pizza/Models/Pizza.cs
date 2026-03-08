namespace restaurent_pizza.Models;

// 🔵 C# pur — Pizza hérite de BaseEntity (comme ActivityReport : Entity au travail)
public class Pizza : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal CostPrice { get; set; }         // 🔒 SECRET — prix d'achat, jamais exposé au client (via DTO plus tard)
    public bool IsAvailable { get; set; } = true;

    // 🔵 C# pur — Factory Method (comme ActivityReport.Create() ou Holiday.Create() au travail)
    // L'entité contrôle sa propre création : Id et CreatedOn sont TOUJOURS remplis correctement
    public static Pizza Create(string name, string description, decimal price)
    {
        return new Pizza
        {
            Id = Guid.NewGuid(),                   // 🔵 Génère un identifiant unique
            Name = name,
            Description = description,
            Price = price,
            CostPrice = 0,
            IsAvailable = true,
            CreatedOn = DateTimeOffset.UtcNow       // 🔵 Timestamp de création (UTC = universel)
        };
    }

    // 🔵 C# pur — Soft Delete (comme WorkContract.Delete() au travail)
    // On ne supprime PAS de la BDD — on marque une date de suppression
    public Pizza Delete()
    {
        if (DeletedOn == null)                     // 🔵 Seulement si pas déjà supprimé
            DeletedOn = DateTimeOffset.UtcNow;
        return this;                               // 🔵 return this = pattern fluent (comme au travail)
    }
}