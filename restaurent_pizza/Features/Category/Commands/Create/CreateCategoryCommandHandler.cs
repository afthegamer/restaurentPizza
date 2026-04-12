using MediatR;
using restaurent_pizza.Data;
using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Features.Category.Commands.Create;

public class CreateCategoryCommandHandler(PizzaDbContext context)
    : IRequestHandler<CreateCategoryCommand, CategoryResult>
{
    public async Task<CategoryResult> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = Models.Category.Create(request.Name, request.Description);

        context.Categories.Add(category);
        await context.SaveChangesAsync(cancellationToken);

        return new CategoryResult(category);
    }
}