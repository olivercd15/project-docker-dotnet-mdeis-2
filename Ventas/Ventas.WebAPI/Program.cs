using Microsoft.OpenApi.Models;
using Ventas.Application.Services;
using Ventas.Infrastructure;
using Ventas.WebAPI;

EnvLoader.Load();
var builder = WebApplication.CreateBuilder(args);

// Add infrastructure (Service Bus)
builder.Services.AddInfrastructure();

// Registrar VentaService
builder.Services.AddScoped<VentaService>(sp =>
{
    var bus = sp.GetRequiredService<Ventas.Domain.Interfaces.IAzureServiceBusClient>();
    var usuariosQueue = Environment.GetEnvironmentVariable("VENTAS_USUARIOS_QUEUE")!;
    var inventarioQueue = Environment.GetEnvironmentVariable("VENTAS_INVENTARIO_QUEUE")!;
    return new VentaService(bus, usuariosQueue, inventarioQueue);
});


builder.Services.AddControllers();

// Configuración de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "VENTAS API",
        Version = "v1",
        Description = "API de Modulo de VENTAS de SISVENT - NOVASOFT 🐯"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "VENTAS API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.Run();
