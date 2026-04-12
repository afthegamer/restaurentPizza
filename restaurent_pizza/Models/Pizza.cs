namespace restaurent_pizza.Models;

// 🔵 C# pur — Pizza hérite de BaseEntity (comme ActivityReport : Entity au travail)
public class Pizza : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal CostPrice { get; set; }         // 🔒 SECRET — prix d'achat, jamais exposé au client (via DTO plus tard)
    public bool IsAvailable { get; set; } = true;
    // 🔵 FK + Navigation — une pizza appartient à une catégorie (relation 1:N)
    public Guid CategoryId { get; set; }               // 🔵 La colonne FK en BDD
    public Category Category { get; set; } = null!;    // 🔵 Navigation property (chargée par Include)


    // 🔵 C# pur — Factory Method (comme ActivityReport.Create() ou Holiday.Create() au travail)
    // L'entité contrôle sa propre création : Id et CreatedOn sont TOUJOURS remplis correctement
    public static Pizza Create(string name, string description, decimal price, Guid categoryId)
    {
        return new Pizza
        {
            Id = Guid.NewGuid(),                   // 🔵 Génère un identifiant unique
            Name = name,
            Description = description,
            Price = price,
            CostPrice = 0,
            IsAvailable = true,
            CategoryId = categoryId,                   // 🔵 Lie la pizza à sa catégorie
            CreatedOn = DateTimeOffset.UtcNow       // 🔵 Timestamp de création (UTC = universel)
        };
    }

    // 🔵 Soft Delete → hérité de BaseEntity.Delete() (commun à toutes les entités)
}