using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using restaurent_pizza.Models;

namespace restaurent_pizza.Services;

// 🟡 JwtBearer — service de génération de tokens JWT
// En production, un Identity Provider externe gère souvent les tokens — ici on le fait nous-mêmes (plus formateur)
public class TokenService(IConfiguration configuration) : ITokenService
{
    public string GenerateToken(User user)
    {
        var secret = configuration["Jwt:Secret"]!;
        var issuer = configuration["Jwt:Issuer"]!;
        var audience = configuration["Jwt:Audience"]!;
        var expirationMinutes = int.Parse(configuration["Jwt:ExpirationInMinutes"]!);

        // 🔵 C# pur — les claims = les informations dans le token
        // Les claims standards d'un JWT : sub (id), email, role
        var claims = new Dictionary<string, object>
        {
            [JwtRegisteredClaimNames.Sub] = user.Id.ToString(),
            [JwtRegisteredClaimNames.Email] = user.Email,
            [ClaimTypes.Role] = user.Role.ToString()
        };

        // 🟡 Microsoft.IdentityModel — clé de signature (HMAC-SHA256, minimum 256 bits)
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 🟡 JsonWebTokenHandler — plus moderne que JwtSecurityTokenHandler (recommandé depuis .NET 8)
        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = issuer,
            Audience = audience,
            Claims = claims,
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
            SigningCredentials = credentials
        };

        var handler = new JsonWebTokenHandler();
        return handler.CreateToken(descriptor);        // 🟡 Retourne le token JWT en string
    }
}
