using MediatR;
using Microsoft.AspNetCore.Mvc;
using restaurent_pizza.Features.Pizza.Commands.Create;
using restaurent_pizza.Features.Pizza.Commands.Delete;
using restaurent_pizza.Features.Pizza.Commands.Update;
using restaurent_pizza.Features.Pizza.Queries.GetAll;
using restaurent_pizza.Features.Pizza.Queries.GetById;
using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Controllers;

[ApiController]                           // 🔴 ASP.NET — active les conventions API (validation auto, binding auto, réponses 400 auto)
[Route("[controller]")]                   // 🔴 ASP.NET — la route = /pizza (le nom de la classe sans "Controller")
public class PizzaController(ISender mediator) : ControllerBase  // 🟡 MediatR — ISender = interface légère (préférée à IMediator, comme au travail)
{
    // GET /pizza → liste des pizzas depuis PostgreSQL
    [HttpGet]
    public async Task<ActionResult<List<PizzaResult>>> GetAll(CancellationToken cancellationToken)
    {
        var pizzas = await mediator.Send(new GetAllPizzasQuery(), cancellationToken);  // 🟡 MediatR — envoie la Query → GetAllPizzasQueryHandler
        return Ok(pizzas);                                 // 🔴 Ok() = HTTP 200 + sérialise en JSON automatiquement
    }
    // ⚠️ Plus besoin de .Where(p => p.DeletedOn == null) !
    // Le Named Query Filter "SoftDelete" dans PizzaConfiguration s'en charge automatiquement.

    // GET /pizza/{id}
    [HttpGet("{id:guid}")]                                 // 🔴 ASP.NET — contrainte de route : doit être un Guid
    public async Task<ActionResult<PizzaResult>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetPizzaByIdQuery(id), cancellationToken);  // 🟡 MediatR — si non trouvé → EntityNotFoundException → GlobalExceptionFilter → 404 ProblemDetails
        return Ok(result);                                 // 🔵 Mapping entité → Result (fait dans le Handler)
    }

    // POST /pizza → crée une pizza en BDD
    [HttpPost]
    public async Task<ActionResult<PizzaResult>> Create([FromBody] CreatePizzaCommand command, CancellationToken cancellationToken)  // 🟡 MediatR — le body EST la Command directement
    {
        // 🟡 FluentValidation — validation automatique via ValidationBehavior (pipeline MediatR)
        // 🔵 Factory Method Pizza.Create() appelé dans le Handler
        var result = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(                            // 🔴 CreatedAtAction = HTTP 201 + header Location
            nameof(GetById),                               // 🔵 nameof() = référence compile-time (pas de string magique)
            new { id = result.Id },
            result
        );
    }

    // PUT /pizza/{id} → modifie une pizza
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePizzaCommand command, CancellationToken cancellationToken)  // 🔴 IActionResult = pas de données retournées
    {
        // 🟡 FluentValidation + 🟡 EF Core — validation (pipeline) et mise à jour dans le Handler
        if (id != command.Id)
            return BadRequest();                           // 🔴 ASP.NET — sécurité : l'Id route doit matcher l'Id body

        await mediator.Send(command, cancellationToken);
        return NoContent();                                // 🔴 NoContent() = HTTP 204 (succès, rien à retourner)
    }

    // DELETE /pizza/{id} → Soft Delete !
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        // 🔵 Soft Delete via la méthode de l'entité (pizza.Delete()) — fait dans le Handler
        await mediator.Send(new DeletePizzaCommand(id), cancellationToken);
        return NoContent();                                // 🔴 HTTP 204
    }
}
