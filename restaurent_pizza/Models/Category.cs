namespace restaurent_pizza.Models;

// 🔵 C# pur — entité Category (comme WorkContractType au travail)
// Hérite de BaseEntity : Id (Guid), CreatedOn, UpdatedOn, DeletedOn
public class Category : BaseEntity
{
    public string Name { get; set; } = string.Empty;           // 🔵 "Classiques", "Spéciales", "Végétariennes"
    public string Description { get; set; } = string.Empty;

    // 🔵 Navigation property — une catégorie contient N pizzas (comme WorkContractType.WorkContracts au travail)
    public ICollection<Pizza> Pizzas { get; set; } = [];

    // 🔵 Factory Method — même pattern que Pizza.Create()
    public static Category Create(string name, string description)
    {
        return new Category
        {
            Name = name,
            Description = description
        };
    }
}