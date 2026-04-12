using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Features.Auth.Commands.Login;

// 🟡 MediatR — Login est une Command (pas une Query) car il génère un token (effet de bord)
public record LoginCommand(string Email, string Password) : ICommand<AuthResult>;
