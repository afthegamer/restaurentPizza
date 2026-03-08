namespace restaurent_pizza.Models.Dtos;

// 🔵 C# pur — Result DTO (comme CarouselInfoResult ou EmployeeResult au travail)
// Nommé "Result" comme au travail (pas "Response" ou "Dto")
public class PizzaResult
{
    // Constructeur mapping depuis l'entité (comme EmployeeResult(Employee employee) au travail)
    // PAS d'AutoMapper — mapping manuel = contrôle total sur ce qui est exposé
    public PizzaResult(Pizza pizza)
    {
        Id = pizza.Id;
        Name = pizza.Name;
        Description = pizza.Description;
        Price = pizza.Price;
        IsAvailable = pizza.IsAvailable;
        IsArchived = pizza.DeletedOn != null;  // Transforme DeletedOn en booléen lisible
        // ⚠️ CostPrice n'est PAS mappé — le client ne le verra JAMAIS
    }

    public PizzaResult() { }  // Constructeur vide requis pour la désérialisation JSON

    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; }
    public bool IsArchived { get; set; }
}