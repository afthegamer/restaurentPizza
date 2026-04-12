using MediatR;
using Microsoft.AspNetCore.Mvc;
using restaurent_pizza.Features.Auth.Commands.Login;
using restaurent_pizza.Features.Auth.Commands.Register;
using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(ISender mediator) : ControllerBase
{
    // POST /auth/register → inscription
    [HttpPost("register")]
    public async Task<ActionResult<AuthResult>> Register([FromBody] RegisterCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }

    // POST /auth/login → connexion
    [HttpPost("login")]
    public async Task<ActionResult<AuthResult>> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return Ok(result);
    }
}