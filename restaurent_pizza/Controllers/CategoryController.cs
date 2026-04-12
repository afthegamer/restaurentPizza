using MediatR;
using Microsoft.AspNetCore.Mvc;
using restaurent_pizza.Features.Category.Commands.Create;
using restaurent_pizza.Features.Category.Commands.Delete;
using restaurent_pizza.Features.Category.Commands.Update;
using restaurent_pizza.Features.Category.Queries.GetAll;
using restaurent_pizza.Features.Category.Queries.GetById;
using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController(ISender mediator) : ControllerBase
{
    // GET /category
    [HttpGet]
    public async Task<ActionResult<List<CategoryResult>>> GetAll(CancellationToken cancellationToken)
    {
        var categories = await mediator.Send(new GetAllCategoriesQuery(), cancellationToken);
        return Ok(categories);
    }

    // GET /category/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoryResult>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetCategoryByIdQuery(id), cancellationToken);
        return Ok(result);
    }

    // POST /category
    [HttpPost]
    public async Task<ActionResult<CategoryResult>> Create([FromBody] CreateCategoryCommand command, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // PUT /category/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
            return BadRequest();

        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    // DELETE /category/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteCategoryCommand(id), cancellationToken);
        return NoContent();
    }
}
