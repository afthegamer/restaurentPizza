using Microsoft.AspNetCore.Mvc;
using restaurent_pizza.Models.Dtos;
using restaurent_pizza.Services;

namespace restaurent_pizza.Controllers;

[ApiController]                           // 🔴 ASP.NET — active les conventions API (validation auto, binding auto, réponses 400 auto)
[Route("[controller]")]                   // 🔴 ASP.NET — la route = /pizza (le nom de la classe sans "Controller")
public class PizzaController(IPizzaService pizzaService) : ControllerBase  // 🔴 ASP.NET — injection du service via primary constructor (le Controller ne connaît plus le DbContext !)
{
    // GET /pizza → liste des pizzas depuis PostgreSQL
    [HttpGet]
    public async Task<ActionResult<List<PizzaResult>>> GetAll(CancellationToken cancellationToken)
    {
        var pizzas = await pizzaService.GetAllAsync(cancellationToken);  // 🔵 Délègue au service (EF Core, mapping, query filter → tout est dans PizzaService)
        return Ok(pizzas);                                 // 🔴 Ok() = HTTP 200 + sérialise en JSON automatiquement
    }
    // ⚠️ Plus besoin de .Where(p => p.DeletedOn == null) !
    // Le Named Query Filter "SoftDelete" dans PizzaConfiguration s'en charge automatiquement.

    // GET /pizza/{id}
    [HttpGet("{id:guid}")]                                 // 🔴 ASP.NET — contrainte de route : doit être un Guid
    public async Task<ActionResult<PizzaResult>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await pizzaService.GetByIdAsync(id, cancellationToken);  // 🔵 Si non trouvé → EntityNotFoundException → GlobalExceptionFilter → 404 ProblemDetails
        return Ok(result);                                 // 🔵 Mapping entité → Result (fait dans le service)
    }

    // POST /pizza → crée une pizza en BDD
    [HttpPost]
    public async Task<ActionResult<PizzaResult>> Create([FromBody] CreatePizzaDto dto, CancellationToken cancellationToken)  // 🔴 [FromBody] = lit le JSON du corps de la requête
    {
        // 🟡 FluentValidation — validation faite dans le service (ValidateAsync, jamais Validate synchrone)
        // 🔵 Factory Method Pizza.Create() appelé dans le service
        var result = await pizzaService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(                            // 🔴 CreatedAtAction = HTTP 201 + header Location
            nameof(GetById),                               // 🔵 nameof() = référence compile-time (pas de string magique)
            new { id = result.Id },
            result
        );
    }

    // PUT /pizza/{id} → modifie une pizza
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePizzaDto dto, CancellationToken cancellationToken)  // 🔴 IActionResult = pas de données retournées
    {
        // 🟡 FluentValidation + 🟡 EF Core — validation et mise à jour dans le service
        await pizzaService.UpdateAsync(id, dto, cancellationToken);
        return NoContent();                                // 🔴 NoContent() = HTTP 204 (succès, rien à retourner)
    }

    // DELETE /pizza/{id} → Soft Delete !
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        // 🔵 Soft Delete via la méthode de l'entité (pizza.Delete()) — fait dans le service
        await pizzaService.DeleteAsync(id, cancellationToken);
        return NoContent();                                // 🔴 HTTP 204
    }
}
