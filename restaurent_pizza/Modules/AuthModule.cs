using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using restaurent_pizza.Services;

namespace restaurent_pizza.Modules;

// 🔴 ASP.NET — module d'enregistrement de l'authentification JWT
// Pattern Module : chaque module regroupe les services d'un domaine
public static class AuthModule
{
    public static IServiceCollection AddAuthModule(this IServiceCollection services, IConfiguration configuration)
    {
        // 🟡 JwtBearer — active l'authentification par token JWT
        // JwtBearerDefaults.AuthenticationScheme = "Bearer" → le header HTTP sera "Authorization: Bearer <token>"
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                // 🟡 JwtBearer — règles de validation du token reçu
                // Chaque requête avec un token passe par ces vérifications AVANT d'atteindre le Controller
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,              // 🟡 Vérifie QUI a émis le token (doit matcher Jwt:Issuer)
                    ValidateAudience = true,             // 🟡 Vérifie À QUI le token est destiné (doit matcher Jwt:Audience)
                    ValidateLifetime = true,             // 🟡 Vérifie que le token n'est pas expiré
                    ValidateIssuerSigningKey = true,      // 🟡 Vérifie que la signature est valide (pas falsifié)
                    ValidIssuer = configuration["Jwt:Issuer"],       // 🔵 Lit la valeur depuis appsettings
                    ValidAudience = configuration["Jwt:Audience"],   // 🔵 Lit la valeur depuis appsettings
                    IssuerSigningKey = new SymmetricSecurityKey(      // 🟡 La clé secrète pour vérifier la signature
                        Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]!))  // 🔵 Même clé que dans TokenService (HMAC-SHA256)
                };
            });

        // 🔴 ASP.NET — active le système d'autorisation (nécessaire pour [Authorize] et [Authorize(Roles = "...")])
        services.AddAuthorization();

        // 🔵 Enregistre le TokenService (génère les JWT lors du login/register)
        services.AddScoped<ITokenService, TokenService>();

        return services;
    }
}
