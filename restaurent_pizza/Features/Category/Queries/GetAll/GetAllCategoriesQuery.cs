using restaurent_pizza.Models.Dtos;

namespace restaurent_pizza.Features.Category.Queries.GetAll;

public record GetAllCategoriesQuery : IQuery<List<CategoryResult>>;