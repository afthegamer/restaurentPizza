using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Services;

// 🔵 C# pur — interface = contrat (comme IPizzaService au travail)
// Définit CE QUE le service fait, pas COMMENT il le fait
// Le Controller dépend de l'interface, pas de l'implémentation → testable + découplé
public interface IPizzaService
{
    Task<List<PizzaResult>> GetAllAsync(CancellationToken cancellationToken);
    Task<PizzaResult> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PizzaResult> CreateAsync(CreatePizzaDto dto, CancellationToken cancellationToken);
    Task UpdateAsync(Guid id, UpdatePizzaDto dto, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}