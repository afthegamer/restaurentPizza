using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restaurent_pizza.Features.Order.Commands.Create;
using restaurent_pizza.Features.Order.Commands.UpdateStatus;
using restaurent_pizza.Features.Order.Queries.GetAll;
using restaurent_pizza.Features.Order.Queries.GetById;
using restaurent_pizza.Features.Order.Queries.GetMyOrders;
using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]                                // 🔴 ASP.NET — tous les endpoints nécessitent un token JWT
public class OrderController(ISender mediator) : ControllerBase
{
    // POST /order → passer une commande (tout user connecté)
    [HttpPost]
    public async Task<ActionResult<OrderResult>> Create([FromBody] CreateOrderDto dto, CancellationToken cancellationToken)
    {
        // 🔴 ASP.NET — récupère l'Id du user connecté depuis le claim "sub" du token JWT
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var command = new CreateOrderCommand(userId, dto.Items);
        var result = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // GET /order → mes commandes (filtrées par user connecté)
    [HttpGet]
    public async Task<ActionResult<List<OrderResult>>> GetMyOrders(CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await mediator.Send(new GetMyOrdersQuery(userId), cancellationToken);
        return Ok(result);
    }

    // GET /order/{id} → détail d'une commande (la sienne ou Admin)
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderResult>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var isAdmin = User.IsInRole("Admin");          // 🔴 ASP.NET — vérifie si le user a le rôle Admin

        var result = await mediator.Send(new GetOrderByIdQuery(id, userId, isAdmin), cancellationToken);
        return Ok(result);
    }

    // GET /order/all → toutes les commandes (Admin only)
    [Authorize(Roles = "Admin")]                       // 🔴 ASP.NET — seul un Admin peut voir toutes les commandes
    [HttpGet("all")]
    public async Task<ActionResult<List<OrderResult>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllOrdersQuery(), cancellationToken);
        return Ok(result);
    }

    // PUT /order/{id}/status → changer le statut (Admin only)
    [Authorize(Roles = "Admin")]                       // 🔴 ASP.NET — seul un Admin peut changer le statut
    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateOrderStatusDto dto, CancellationToken cancellationToken)
    {
        var command = new UpdateOrderStatusCommand(id, dto.Status);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }
}
