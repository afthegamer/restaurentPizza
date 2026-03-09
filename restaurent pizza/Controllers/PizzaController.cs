using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restaurent_pizza.Data;
using restaurent_pizza.Models;
using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Controllers;

[ApiController]                           // 🔴 ASP.NET — active les conventions API (validation auto, binding auto, réponses 400 auto)
[Route("[controller]")]                   // 🔴 ASP.NET — la route = /pizza (le nom de la classe sans "Controller")
public class PizzaController(PizzaDbContext context) : ControllerBase  // 🔴 ASP.NET + 🟡 EF Core — injection du DbContext via primary constructor
{
    // GET /pizza → liste des pizzas depuis PostgreSQL
    [HttpGet]
    public async Task<ActionResult<List<PizzaResult>>> GetAll(CancellationToken cancellationToken)
    {
        var pizzas = await context.Pizzas                  // 🟡 EF Core — accès à la table Pizzas
            .Select(p => new PizzaResult(p))               // 🔵 Mapping entité → DTO Result
            .ToListAsync(cancellationToken);               // 🟡 EF Core — exécute la requête SQL (async)
        return Ok(pizzas);                                 // 🔴 Ok() = HTTP 200 + sérialise en JSON automatiquement
    }
    // ⚠️ Plus besoin de .Where(p => p.DeletedOn == null) !
    // Le Named Query Filter "SoftDelete" dans PizzaConfiguration s'en charge automatiquement.

    // GET /pizza/{id}
    [HttpGet("{id:guid}")]                                 // 🔴 ASP.NET — contrainte de route : doit être un Guid
    public async Task<ActionResult<PizzaResult>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var pizza = await context.Pizzas
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);  // 🟡 EF Core — SELECT WHERE Id = ...
        if (pizza == null)
            return NotFound();                             // 🔴 NotFound() = HTTP 404
        return Ok(new PizzaResult(pizza));                 // 🔵 Mapping entité → Result
    }

    // POST /pizza → crée une pizza en BDD
    [HttpPost]
    public async Task<ActionResult<PizzaResult>> Create([FromBody] CreatePizzaDto dto, CancellationToken cancellationToken)  // 🔴 [FromBody] = lit le JSON du corps de la requête
    {
        var pizza = Pizza.Create(dto.Name, dto.Description, dto.Price);  // 🔵 Factory Method !
        context.Pizzas.Add(pizza);                         // 🟡 EF Core — prépare l'INSERT
        await context.SaveChangesAsync(cancellationToken); // 🟡 EF Core — exécute l'INSERT + auto-timestamp CreatedOn
        return CreatedAtAction(                            // 🔴 CreatedAtAction = HTTP 201 + header Location
            nameof(GetById),                               // 🔵 nameof() = référence compile-time (pas de string magique)
            new { id = pizza.Id },
            new PizzaResult(pizza)
        );
    }

    // PUT /pizza/{id} → modifie une pizza
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePizzaDto dto, CancellationToken cancellationToken)  // 🔴 IActionResult = pas de données retournées
    {
        var pizza = await context.Pizzas
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (pizza == null) return NotFound();

        pizza.Name = dto.Name;                             // 🔵 Mise à jour des propriétés
        pizza.Description = dto.Description;
        pizza.Price = dto.Price;
        pizza.IsAvailable = dto.IsAvailable;
        await context.SaveChangesAsync(cancellationToken); // 🟡 EF Core — exécute l'UPDATE + auto-timestamp UpdatedOn
        return NoContent();                                // 🔴 NoContent() = HTTP 204 (succès, rien à retourner)
    }

    // DELETE /pizza/{id} → Soft Delete !
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var pizza = await context.Pizzas
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        if (pizza == null) return NotFound();

        pizza.Delete();                                    // 🔵 Soft Delete via la méthode de l'entité !
        await context.SaveChangesAsync(cancellationToken); // 🟡 EF Core — exécute l'UPDATE (DeletedOn rempli)
        return NoContent();                                // 🔴 HTTP 204
    }
}
