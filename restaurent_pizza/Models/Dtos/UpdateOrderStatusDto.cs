namespace restaurent_pizza.Models.Dtos;

// 🔵 C# pur — ce que l'admin envoie pour changer le statut d'une commande
public record UpdateOrderStatusDto(
    string Status                        // 🔵 Le nouveau statut en string ("Preparing", "Ready", "Delivered", "Cancelled")
);
