namespace restaurent_pizza.Exceptions;

// 🔵 C# pur — exception custom pour les conflits de données
// Lancée quand on tente de créer une ressource qui existe déjà (ex: email dupliqué)
// → Mappée en HTTP 409 Conflict par le GlobalExceptionFilter
public class ConflictException : Exception
{
    public ConflictException(string message)
        : base(message) { }
}
