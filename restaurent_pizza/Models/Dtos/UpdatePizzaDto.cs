namespace restaurent_pizza.Models.Dtos;

// 🔵 C# pur — DTO pour la modification (ajoute IsAvailable par rapport au Create)
public record UpdatePizzaDto(
    string Name,
    string Description,
    decimal Price,
    bool IsAvailable
);