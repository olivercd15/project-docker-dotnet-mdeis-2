using Microsoft.OpenApi.Models;
using Usuarios.Application.Services;
using Usuarios.Infrastructure.Messaging;
using Usuarios.WebAPI;

EnvLoader.Load();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<UsuarioService>();
builder.Services.AddControllers();

// Configuración de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "USUARIOS API",
        Version = "v1",
        Description = "API de Modulo de USUARIOS de SISVENT - NOVASOFT 🐯"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "USUARIOS API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Listener Service Bus
var listener = new UsuarioListener();
await listener.StartAsync();

app.Run();
