namespace restaurent_pizza.Models;

// 🔵 C# pur — enum de statut (pattern classique : chaque entité métier a son cycle de vie)
// Chaque commande passe par ces étapes dans l'ordre
public enum OrderStatus
{
    Pending = 0,        // Commande reçue, en attente de traitement
    Preparing = 1,      // En cours de préparation en cuisine
    Ready = 2,          // Prête à être récupérée/livrée
    Delivered = 3,      // Livrée au client
    Cancelled = 4       // Annulée
}
