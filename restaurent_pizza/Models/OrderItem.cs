namespace restaurent_pizza.Models;

// 🔵 C# pur — une ligne de commande (pattern classique : Order 1→N OrderItem)
// Stocke un SNAPSHOT du prix et du nom au moment de la commande
public class OrderItem : BaseEntity
{
    // 🔵 FK + Navigation — cette ligne appartient à une commande (relation N:1)
    public Guid OrderId { get; set; }              // 🔵 La colonne FK en BDD
    public Order Order { get; set; } = null!;      // 🔵 Navigation vers la commande parente

    // 🔵 FK + Navigation — cette ligne référence une pizza (relation N:1)
    public Guid PizzaId { get; set; }              // 🔵 La colonne FK en BDD
    public Pizza Pizza { get; set; } = null!;      // 🔵 Navigation vers la pizza

    public int Quantity { get; set; }              // 🔵 Combien de cette pizza
    public decimal UnitPrice { get; set; }         // 🔵 SNAPSHOT — prix de la pizza AU MOMENT de la commande
    public string PizzaName { get; set; } = string.Empty;  // 🔵 SNAPSHOT — nom de la pizza au moment de la commande

    // 🔵 Propriété calculée — total de cette ligne (pas stocké en BDD)
    public decimal LineTotal => Quantity * UnitPrice;

    // 🔵 Factory Method — crée une ligne depuis une pizza (snapshot automatique)
    public static OrderItem Create(Pizza pizza, int quantity)
    {
        return new OrderItem
        {
            Id = Guid.NewGuid(),
            PizzaId = pizza.Id,
            Quantity = quantity,
            UnitPrice = pizza.Price,          // 🔵 SNAPSHOT — capture le prix actuel
            PizzaName = pizza.Name,           // 🔵 SNAPSHOT — capture le nom actuel
            CreatedOn = DateTimeOffset.UtcNow
        };
    }
}
