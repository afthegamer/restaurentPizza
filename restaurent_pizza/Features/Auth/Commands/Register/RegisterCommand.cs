using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Features.Auth.Commands.Register;

public record RegisterCommand(string Email, string Password, string FirstName, string LastName) : ICommand<AuthResult>;
