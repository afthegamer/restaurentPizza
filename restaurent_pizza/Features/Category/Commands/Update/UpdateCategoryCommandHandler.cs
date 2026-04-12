using MediatR;
using Microsoft.EntityFrameworkCore;
using restaurent_pizza.Data;
using restaurent_pizza.Exceptions;

namespace restaurent_pizza.Features.Category.Commands.Update;

public class UpdateCategoryCommandHandler(PizzaDbContext context)
    : IRequestHandler<UpdateCategoryCommand>
{
    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await context.Categories
                           .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
                       ?? throw new EntityNotFoundException("Category", request.Id);

        category.Name = request.Name;
        category.Description = request.Description;

        await context.SaveChangesAsync(cancellationToken);
    }
}