namespace restaurent_pizza.Models.Dtos;

// 🔵 C# pur — record = immutable, parfait pour les DTOs d'entrée
public record CreateCategoryDto(string Name, string Description);