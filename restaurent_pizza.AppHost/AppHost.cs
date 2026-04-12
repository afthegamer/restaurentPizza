var builder = DistributedApplication.CreateBuilder(args);

// 🐳 PostgreSQL géré par docker-compose (pas par Aspire)
// La connection string est dans appsettings.json
var pizzaDb = builder.AddConnectionString("pizzadb");

// 🍕 API — projet .NET
var api = builder.AddProject<Projects.restaurent_pizza>("api")
    .WithReference(pizzaDb);

// 🟡 Aspire — Frontend Angular
// AddJavaScriptApp lance le script npm "start" (= "ng serve") dans le dossier Angular
// .WithNpm() configure npm comme package manager (auto-install des dépendances)
// .WithHttpEndpoint(env: "PORT") injecte le port dynamique — Angular CLI respecte PORT automatiquement
// .WaitFor(api) attend que l'API soit healthy avant de lancer le frontend
// .WithExternalHttpEndpoints() rend l'URL cliquable dans le Dashboard Aspire
builder.AddJavaScriptApp("frontend", "../restaurent-pizza-front", "start")
    .WithNpm()
    .WithReference(api)
    .WaitFor(api)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints();

builder.Build().Run();
