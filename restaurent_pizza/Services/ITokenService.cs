using restaurent_pizza.Models;

namespace restaurent_pizza.Services;

// 🔵 C# pur — interface pour la génération de tokens JWT
public interface ITokenService
{
    string GenerateToken(User user);
}
