var builder = DistributedApplication.CreateBuilder(args);

// 🐳 PostgreSQL géré par docker-compose (pas par Aspire)
// La connection string est dans appsettings.json
var pizzaDb = builder.AddConnectionString("pizzadb");

// 🍕 API
builder.AddProject<Projects.restaurent_pizza>("api")
    .WithReference(pizzaDb);

builder.Build().Run();
