var builder = DistributedApplication.CreateBuilder(args);

// 🐳 Crée un serveur PostgreSQL (Aspire le lance dans Docker automatiquement)
var postgres = builder.AddPostgres("postgres");

// 🗄️ Crée une base de données "pizzadb" dans ce serveur
var pizzaDb = postgres.AddDatabase("pizzadb");

// 🍕 Ajoute ton API comme projet à orchestrer
builder.AddProject<Projects.restaurent_pizza>("api")
    .WithReference(pizzaDb)     // Passe la connection string automatiquement
    .WaitFor(pizzaDb);          // Attend que PostgreSQL soit prêt avant de lancer l'API

builder.Build().Run();