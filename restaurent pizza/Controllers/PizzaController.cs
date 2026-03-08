using Microsoft.AspNetCore.Mvc;
using restaurent_pizza.Models;
using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Controllers;

[ApiController]                           // 🔴 ASP.NET — active les conventions API (validation auto, binding auto, réponses 400 auto)
[Route("[controller]")]                   // 🔴 ASP.NET — la route = /pizza (le nom de la classe sans "Controller")
public class PizzaController : ControllerBase  // 🔴 ASP.NET — classe de base pour les API (pas Controller, qui est pour MVC avec vues)
{
    // 🔵 C# pur — maintenant une liste de VRAIS objets Pizza (plus de simples strings)
    // Temporaire en mémoire (on remplacera par PostgreSQL en Phase 4)
    private static readonly List<Pizza> Pizzas =
    [
        Pizza.Create("Margherita", "Tomate, mozzarella, basilic", 9.50m),
        Pizza.Create("Pepperoni", "Tomate, mozzarella, pepperoni", 11.00m),
        Pizza.Create("Quatre Fromages", "Mozzarella, gorgonzola, parmesan, chèvre", 12.50m)
    ];

    // 🔴 ASP.NET — GET /pizza → retourne des PizzaResult (pas des entités !)
    [HttpGet]
    public ActionResult<List<PizzaResult>> GetAll()
    {
        var results = Pizzas
            .Where(p => p.DeletedOn == null)          // 🔵 Filtre soft delete (comme au travail)
            .Select(p => new PizzaResult(p))           // 🔵 Mapping entité → DTO Result
            .ToList();
        return Ok(results);                            // 🔴 Ok() = HTTP 200 + sérialise en JSON automatiquement
    }

    // 🔴 ASP.NET — GET /pizza/{id} → par Guid maintenant (plus par index int)
    [HttpGet("{id:guid}")]                             // 🔴 ASP.NET — contrainte de route : doit être un Guid
    public ActionResult<PizzaResult> GetById(Guid id)
    {
        var pizza = Pizzas.FirstOrDefault(p => p.Id == id && p.DeletedOn == null);
        if (pizza == null)
            return NotFound();                         // 🔴 NotFound() = HTTP 404
        return Ok(new PizzaResult(pizza));             // 🔵 Mapping entité → Result
    }

    // 🔴 ASP.NET — POST /pizza → reçoit un CreatePizzaDto, retourne un PizzaResult
    [HttpPost]
    public ActionResult<PizzaResult> Create([FromBody] CreatePizzaDto dto)  // 🔴 [FromBody] = lit le JSON du corps de la requête
    {
        var pizza = Pizza.Create(dto.Name, dto.Description, dto.Price);  // 🔵 Factory Method !
        Pizzas.Add(pizza);                             // 🔵 C# pur — ajoute à la liste
        return CreatedAtAction(                        // 🔴 CreatedAtAction = HTTP 201 + header Location
            nameof(GetById),                           // 🔵 nameof() = référence compile-time (pas de string magique)
            new { id = pizza.Id },
            new PizzaResult(pizza)                     // Retourne le Result, pas l'entité
        );
    }

    // 🔴 ASP.NET — PUT /pizza/{id} → reçoit un UpdatePizzaDto
    [HttpPut("{id:guid}")]
    public IActionResult Update(Guid id, [FromBody] UpdatePizzaDto dto)  // 🔴 IActionResult = pas de données retournées
    {
        var pizza = Pizzas.FirstOrDefault(p => p.Id == id && p.DeletedOn == null);
        if (pizza == null) return NotFound();

        pizza.Name = dto.Name;                         // 🔵 Mise à jour manuelle des propriétés
        pizza.Description = dto.Description;
        pizza.Price = dto.Price;
        pizza.IsAvailable = dto.IsAvailable;
        return NoContent();                            // 🔴 NoContent() = HTTP 204 (succès, rien à retourner)
    }

    // 🔴 ASP.NET — DELETE /pizza/{id} → Soft Delete ! (pas de suppression physique)
    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id)
    {
        var pizza = Pizzas.FirstOrDefault(p => p.Id == id && p.DeletedOn == null);
        if (pizza == null) return NotFound();

        pizza.Delete();                                // 🔵 Soft Delete via la méthode de l'entité !
        return NoContent();                            // 🔴 HTTP 204
    }
}
