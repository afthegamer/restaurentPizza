using MediatR;
using Microsoft.EntityFrameworkCore;
using restaurent_pizza.Data;
using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Features.Category.Queries.GetAll;

public class GetAllCategoriesQueryHandler(PizzaDbContext context)
    : IRequestHandler<GetAllCategoriesQuery, List<CategoryResult>>
{
    public async Task<List<CategoryResult>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await context.Categories
            .Select(c => new CategoryResult(c))
            .ToListAsync(cancellationToken);
    }
}