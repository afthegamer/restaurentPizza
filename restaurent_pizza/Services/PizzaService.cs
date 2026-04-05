using FluentValidation;
using Microsoft.EntityFrameworkCore;
using restaurent_pizza.Data;
using restaurent_pizza.Exceptions;
using restaurent_pizza.Models;
using restaurent_pizza.Models.Dtos;
using restaurent_pizza.Validators;

namespace restaurent_pizza.Services;

// 🔵 C# pur — implémentation du contrat IPizzaService
// Contient toute la logique métier (validation, accès BDD, mapping)
// Primary constructor : injection du DbContext (comme au travail)
public class PizzaService(PizzaDbContext context) : IPizzaService
{
    public async Task<List<PizzaResult>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Pizzas
            .Select(p => new PizzaResult(p))
            .ToListAsync(cancellationToken);
    }

    public async Task<PizzaResult> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var pizza = await context.Pizzas
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken)
            ?? throw new EntityNotFoundException("Pizza", id);
        return new PizzaResult(pizza);
    }

    public async Task<PizzaResult> CreateAsync(CreatePizzaDto dto, CancellationToken cancellationToken)
    {
        // 🟡 FluentValidation — validation manuelle (ValidateAsync, jamais Validate synchrone)
        var validator = new CreatePizzaValidator();
        var validationResult = await validator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);  // 🔵 Lance une exception → GlobalExceptionFilter

        var pizza = Pizza.Create(dto.Name, dto.Description, dto.Price);
        context.Pizzas.Add(pizza);
        await context.SaveChangesAsync(cancellationToken);
        return new PizzaResult(pizza);
    }

    public async Task UpdateAsync(Guid id, UpdatePizzaDto dto, CancellationToken cancellationToken)
    {
        // 🟡 FluentValidation — validation manuelle
        var validator = new UpdatePizzaValidator();
        var validationResult = await validator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var pizza = await context.Pizzas
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken)
            ?? throw new EntityNotFoundException("Pizza", id);

        pizza.Name = dto.Name;
        pizza.Description = dto.Description;
        pizza.Price = dto.Price;
        pizza.IsAvailable = dto.IsAvailable;
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var pizza = await context.Pizzas
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken)
            ?? throw new EntityNotFoundException("Pizza", id);

        pizza.Delete();
        await context.SaveChangesAsync(cancellationToken);
    }
}
