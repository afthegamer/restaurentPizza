using MediatR;
using Microsoft.EntityFrameworkCore;
using restaurent_pizza.Data;
using restaurent_pizza.Exceptions;

namespace restaurent_pizza.Features.Category.Commands.Delete;

public class DeleteCategoryCommandHandler(PizzaDbContext context)
    : IRequestHandler<DeleteCategoryCommand>
{
    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await context.Categories
                           .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken)
                       ?? throw new EntityNotFoundException("Category", request.Id);

        category.Delete();
        await context.SaveChangesAsync(cancellationToken);
    }
}