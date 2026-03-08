using Microsoft.AspNetCore.Mvc;

namespace restaurent_pizza.Controllers;

[ApiController]                           // 🔴 ASP.NET — active les conventions API (validation auto, binding auto, réponses 400 auto)
[Route("[controller]")]                   // 🔴 ASP.NET — la route = /pizza (le nom de la classe sans "Controller")
public class PizzaController : ControllerBase  // 🔴 ASP.NET — classe de base pour les API (pas Controller, qui est pour MVC avec vues)
{
    // 🔵 C# pur — liste temporaire en mémoire (on remplacera par PostgreSQL en Phase 4)
    private static readonly List<string> Pizzas = new()
    {
        "Margherita", "Pepperoni", "Quatre Fromages"
    };

    // 🔴 ASP.NET — GET /pizza → retourne toute la liste
    [HttpGet]
    public ActionResult<List<string>> GetAll()
    {
        return Ok(Pizzas);               // 🔴 Ok() = HTTP 200 + sérialise en JSON automatiquement
    }

    // 🔴 ASP.NET — GET /pizza/0 → retourne une pizza par son index
    [HttpGet("{id}")]
    public ActionResult<string> GetById(int id)
    {
        if (id < 0 || id >= Pizzas.Count)
            return NotFound();            // 🔴 NotFound() = HTTP 404
        return Ok(Pizzas[id]);
    }

    // 🔴 ASP.NET — POST /pizza → ajoute une pizza
    [HttpPost]
    public ActionResult<string> Create([FromBody] string name)  // 🔴 [FromBody] = lit le JSON du corps de la requête
    {
        Pizzas.Add(name);                 // 🔵 C# pur — ajoute à la liste
        return CreatedAtAction(           // 🔴 CreatedAtAction = HTTP 201 + header Location
            nameof(GetById),              // 🔵 nameof() = référence compile-time (pas de string magique)
            new { id = Pizzas.Count - 1 },
            name
        );
    }

    // 🔴 ASP.NET — PUT /pizza/1 → modifie une pizza
    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] string name)  // 🔴 IActionResult = pas de données retournées
    {
        if (id < 0 || id >= Pizzas.Count) return NotFound();
        Pizzas[id] = name;
        return NoContent();               // 🔴 NoContent() = HTTP 204 (succès, rien à retourner)
    }

    // 🔴 ASP.NET — DELETE /pizza/1 → supprime une pizza
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        if (id < 0 || id >= Pizzas.Count) return NotFound();
        Pizzas.RemoveAt(id);
        return NoContent();               // 🔴 HTTP 204
    }
}
