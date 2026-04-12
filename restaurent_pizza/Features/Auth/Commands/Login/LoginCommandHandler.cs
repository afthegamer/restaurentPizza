using MediatR;
using Microsoft.EntityFrameworkCore;
using restaurent_pizza.Data;
using restaurent_pizza.Exceptions;
using restaurent_pizza.Models.Dtos;
using restaurent_pizza.Services;

namespace restaurent_pizza.Features.Auth.Commands.Login;

// 🟡 MediatR — Handler de connexion
public class LoginCommandHandler(PizzaDbContext context, ITokenService tokenService)
    : IRequestHandler<LoginCommand, AuthResult>
{
    public async Task<AuthResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // 🔵 Cherche le user par email
        var user = await context.Users
            .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        // 🟡 BCrypt — vérifie le mot de passe (compare le hash)
        // ⚠️ Message volontairement vague (sécurité : ne pas dire si c'est l'email ou le password qui est faux)
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new AuthenticationException("Email ou mot de passe incorrect.");

        // 🟡 JWT — génère le token
        var token = tokenService.GenerateToken(user);

        return new AuthResult
        {
            Token = token,
            Email = user.Email,
            FirstName = user.FirstName,
            Role = user.Role.ToString()
        };
    }
}
