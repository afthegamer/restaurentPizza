using Microsoft.EntityFrameworkCore;
using restaurent_pizza.Data;
using restaurent_pizza.Filters;
using restaurent_pizza.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

// 🔴 ASP.NET — enregistre le GlobalExceptionFilter (intercepte toutes les exceptions)
// 🔴 ASP.NET — enregistre le système de Controllers
builder.Services.AddControllers(options =>
{
    options.Filters.Add(typeof(GlobalExceptionFilter));
});
builder.Services.AddDatabaseModule();  // après AddControllers(...)
builder.Services.AddMediatRModule();   // 🟡 MediatR + FluentValidation — scanne tous les Handlers et Validators
builder.Services.AddAuthModule(builder.Configuration);

builder.AddNpgsqlDbContext<PizzaDbContext>("pizzadb");  // 🟡 Aspire — connecte EF Core à PostgreSQL

var app = builder.Build();

app.MapDefaultEndpoints();
app.UseAuthentication();    // 🔴 ASP.NET — vérifie le token JWT (AVANT MapControllers !)
app.UseAuthorization();     // 🔴 ASP.NET — vérifie les rôles (AVANT MapControllers !)
app.MapControllers();               // 🔴 ASP.NET — connecte les routes des Controllers

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 🟡 EF Core — applique les migrations automatiquement au démarrage (dev uniquement)
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<PizzaDbContext>();
    await db.Database.MigrateAsync();
}

await app.RunAsync();
