namespace restaurent_pizza.Exceptions;

// 🔵 C# pur — exception custom → interceptée par GlobalExceptionFilter → HTTP 404
// Lancée quand on cherche une entité par Id et qu'elle n'existe pas
public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string entityName, Guid id)
        : base($"{entityName} with id '{id}' was not found.") { }
}