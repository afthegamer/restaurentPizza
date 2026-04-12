namespace restaurent_pizza.Models.Dtos;

// 🔵 C# pur — DTO de sortie (comme PizzaResult, constructeur mapping depuis l'entité)
public class CategoryResult
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsArchived { get; set; }

    public CategoryResult(Category category)
    {
        Id = category.Id;
        Name = category.Name;
        Description = category.Description;
        IsArchived = category.DeletedOn != null;
    }
}