using Microsoft.EntityFrameworkCore;
using restaurent_pizza.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();        

builder.Services.AddOpenApi();

builder.Services.AddControllers();  // 🔴 ASP.NET — enregistre le système de Controllers

builder.AddNpgsqlDbContext<PizzaDbContext>("pizzadb");  // 🟡 Aspire — connecte EF Core à PostgreSQL

var app = builder.Build();

app.MapDefaultEndpoints();           
app.MapControllers();               // 🔴 ASP.NET — connecte les routes des Controllers

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// 🟡 EF Core — applique les migrations au démarrage (comme au travail)
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<PizzaDbContext>();
    await db.Database.MigrateAsync();
}

await app.RunAsync();