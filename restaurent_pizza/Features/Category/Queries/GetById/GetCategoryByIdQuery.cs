using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Features.Category.Queries.GetById;

public record GetCategoryByIdQuery(Guid Id) : IQuery<CategoryResult>;