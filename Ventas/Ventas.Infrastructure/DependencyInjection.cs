using Microsoft.Extensions.DependencyInjection;
using Ventas.Domain.Interfaces;
using Ventas.Infrastructure.Messaging;

namespace Ventas.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            var connectionString = Environment.GetEnvironmentVariable("AZURE_CONNECTION")!;
            var responseQueue = Environment.GetEnvironmentVariable("VENTAS_RESPONSE_QUEUE")!;

            services.AddSingleton<IAzureServiceBusClient>(new AzureServiceBusClient(
                connectionString,
                responseQueue
            ));

            return services;
        }
    }
}
