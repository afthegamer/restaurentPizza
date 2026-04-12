namespace restaurent_pizza.Models.Dtos;

// 🔵 C# pur — réponse après login/register (le token + infos user)
public class AuthResult
{
    public string Token { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
