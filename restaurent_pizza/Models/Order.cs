namespace restaurent_pizza.Models;

// 🔵 C# pur — une commande client (entité avec statut, relations, et méthodes métier)
public class Order : BaseEntity
{
    // 🔵 FK + Navigation — cette commande appartient à un client (relation N:1)
    public Guid UserId { get; set; }               // 🔵 La colonne FK vers l'utilisateur qui a passé la commande
    public User User { get; set; } = null!;        // 🔵 Navigation vers le client

    public OrderStatus Status { get; set; } = OrderStatus.Pending;  // 🔵 Statut de la commande
    public decimal Total { get; set; }             // 🔵 Total STOCKÉ — calculé à la création, figé ensuite

    // 🔵 Collection de navigation — les lignes de cette commande (relation 1:N)
    public List<OrderItem> Items { get; set; } = [];

    // 🔵 Factory Method — crée une commande avec ses lignes et calcule le total
    public static Order Create(Guid userId, List<OrderItem> items)
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Status = OrderStatus.Pending,
            Items = items,
            CreatedOn = DateTimeOffset.UtcNow
        };

        // 🔵 Calcule et stocke le total depuis les lignes
        order.Total = items.Sum(item => item.LineTotal);

        return order;
    }

    // 🔵 Méthode métier — changer le statut (l'entité encapsule ses propres transitions d'état)
    public void UpdateStatus(OrderStatus newStatus)
    {
        Status = newStatus;
        UpdatedOn = DateTimeOffset.UtcNow;
    }
}
