namespace restaurent_pizza.Models.Dtos;

// 🔵 C# pur — record = type immutable pour transporter des données
// Input DTO en record immutable (le client envoie ça, pas l'entité complète)
// Le client envoie SEULEMENT ces 3 champs — pas de Id, pas de CostPrice, pas de CreatedOn
public record CreatePizzaDto(
    string Name,
    string Description,
    decimal Price
);