var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();        

builder.Services.AddOpenApi();

builder.Services.AddControllers();  // 🔴 ASP.NET — enregistre le système de Controllers


var app = builder.Build();

app.MapDefaultEndpoints();           
app.MapControllers();               // 🔴 ASP.NET — connecte les routes des Controllers

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();