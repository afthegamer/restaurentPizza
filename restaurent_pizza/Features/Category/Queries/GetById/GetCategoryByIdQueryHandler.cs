using MediatR;
using Microsoft.EntityFrameworkCore;
using restaurent_pizza.Data;
using restaurent_pizza.Exceptions;
using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Features.Category.Queries.GetById;

public class GetCategoryByIdQueryHandler(PizzaDbContext context)
    : IRequestHandler<GetCategoryByIdQuery, CategoryResult>
{
    public async Task<CategoryResult> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await context.Categories
                           .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
                       ?? throw new EntityNotFoundException("Category", request.Id);

        return new CategoryResult(category);
    }
}