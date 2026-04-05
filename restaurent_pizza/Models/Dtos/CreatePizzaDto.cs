namespace restaurent_pizza.Models.Dtos;

// 🔵 C# pur — record = type immutable pour transporter des données
// Comme CarouselInfoDTO au travail, mais en version moderne (record au lieu de class)
// Le client envoie SEULEMENT ces 3 champs — pas de Id, pas de CostPrice, pas de CreatedOn
public record CreatePizzaDto(
    string Name,
    string Description,
    decimal Price
);