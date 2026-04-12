namespace restaurent_pizza.Models.Dtos;

// 🔵 C# pur — ce que le client envoie pour passer commande
// Record immutable — le client dit juste : "je veux 2 Margherita et 1 Pepperoni"
public record CreateOrderDto(
    List<CreateOrderItemDto> Items       // 🔵 La liste des pizzas commandées
);

// 🔵 C# pur — une ligne de la commande (quelle pizza, quelle quantité)
public record CreateOrderItemDto(
    Guid PizzaId,                        // 🔵 L'Id de la pizza à commander
    int Quantity                         // 🔵 Combien de cette pizza
);
