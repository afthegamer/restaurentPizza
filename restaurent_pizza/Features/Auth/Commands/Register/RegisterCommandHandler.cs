using MediatR;
using Microsoft.EntityFrameworkCore;
using restaurent_pizza.Data;
using restaurent_pizza.Exceptions;
using restaurent_pizza.Models;
using restaurent_pizza.Models.Dtos;
using restaurent_pizza.Services;

namespace restaurent_pizza.Features.Auth.Commands.Register;

// 🟡 MediatR — Handler d'inscription
public class RegisterCommandHandler(PizzaDbContext context, ITokenService tokenService)
    : IRequestHandler<RegisterCommand, AuthResult>
{
    public async Task<AuthResult> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        // 🔵 Vérifie que l'email n'est pas déjà pris
        var exists = await context.Users
            .AnyAsync(u => u.Email == request.Email, cancellationToken);
        if (exists)
            throw new ConflictException("Un compte avec cet email existe déjà.");

        // 🟡 BCrypt — hash le mot de passe (jamais stocké en clair !)
        var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        // 🔵 Factory Method
        var user = User.Create(request.Email, passwordHash, request.FirstName, request.LastName);

        context.Users.Add(user);
        await context.SaveChangesAsync(cancellationToken);

        // 🟡 JWT — génère le token pour connecter l'utilisateur directement après inscription
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
