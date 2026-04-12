namespace restaurent_pizza.Models.Dtos;

// 🔵 C# pur — Result DTO d'une commande (ce que l'API retourne au client)
// Constructeur mapping depuis l'entité — contrôle total sur ce qui est exposé
public class OrderResult
{
    public OrderResult(Order order)
    {
        Id = order.Id;
        Status = order.Status.ToString();          // 🔵 Enum → string ("Pending", "Preparing"...)
        Total = order.Total;
        CreatedOn = order.CreatedOn;
        Items = order.Items.Select(i => new OrderItemResult(i)).ToList();
    }

    public OrderResult() { }

    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public List<OrderItemResult> Items { get; set; } = [];
    // ⚠️ Pas de UserId exposé ! Le client n'a pas besoin de voir ça.
}

// 🔵 C# pur — Result DTO d'une ligne de commande
public class OrderItemResult
{
    public OrderItemResult(OrderItem item)
    {
        Id = item.Id;
        PizzaId = item.PizzaId;
        PizzaName = item.PizzaName;        // 🔵 Le snapshot, pas le nom actuel de la pizza
        Quantity = item.Quantity;
        UnitPrice = item.UnitPrice;        // 🔵 Le snapshot, pas le prix actuel
        LineTotal = item.LineTotal;        // 🔵 Calculé : Quantity * UnitPrice
    }

    public OrderItemResult() { }

    public Guid Id { get; set; }
    public Guid PizzaId { get; set; }
    public string PizzaName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
}
