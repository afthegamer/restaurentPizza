namespace restaurent_pizza.Exceptions;

// 🔵 C# pur — exception custom pour les erreurs d'authentification
// Lancée quand les identifiants sont invalides (email ou mot de passe incorrect)
// → Mappée en HTTP 401 Unauthorized par le GlobalExceptionFilter
public class AuthenticationException : Exception
{
    public AuthenticationException(string message)
        : base(message) { }
}
