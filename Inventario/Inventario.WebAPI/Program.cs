using Inventario.Application.Services;
using Inventario.Infrastructure.Messaging;
using Inventario.WebAPI;
using Microsoft.OpenApi.Models;


EnvLoader.Load();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ProductoService>();
builder.Services.AddControllers();

// Configuración de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "INVENTARIO API",
        Version = "v1",
        Description = "API de Modulo de INVENTARIO de SISVENT - NOVASOFT 🐯"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "INVENTARIO API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Inventario Listener Service Bus
var listener = new ProductoListener();
await listener.StartAsync();



app.Run();
