using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Inventario.Application.Services;
using Microsoft.Extensions.Configuration;

namespace Inventario.Infrastructure.Messaging
{
    public class ProductoListener
    {
        private readonly ServiceBusClient _client;
        private readonly string _requestQueue;
        private readonly string _responseQueue;
        private readonly ProductoService _productoService;

        public ProductoListener()
        {
            var connectionString = Environment.GetEnvironmentVariable("AZURE_CONNECTION")!;
            _requestQueue = Environment.GetEnvironmentVariable("INVENTARIO_REQUEST_QUEUE")!;
            _responseQueue = Environment.GetEnvironmentVariable("INVENTARIO_RESPONSE_QUEUE")!;
            _client = new ServiceBusClient(connectionString);
            _productoService = new ProductoService();
        }

        public async Task StartAsync()
        {
            var processor = _client.CreateProcessor(_requestQueue, new ServiceBusProcessorOptions());

            processor.ProcessMessageAsync += async args =>
            {
                try
                {
                    var body = args.Message.Body.ToString();
                    var data = JsonSerializer.Deserialize<RequestMessage>(body);

                    if (data?.Action == "GetProductos")
                    {
                        var productos = _productoService.GetProductos();
                        var json = JsonSerializer.Serialize(productos);

                        var sender = _client.CreateSender(_responseQueue);
                        await sender.SendMessageAsync(new ServiceBusMessage(json));
                    }

                    await args.CompleteMessageAsync(args.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error procesando mensaje: {ex.Message}");
                }
            };

            processor.ProcessErrorAsync += async args =>
            {
                Console.WriteLine($"Error del listener: {args.Exception.Message}");
                await Task.CompletedTask;
            };

            await processor.StartProcessingAsync();

            Console.WriteLine($"Listener de Productos escuchando en '{_requestQueue}'...");
        }
    }
    public class RequestMessage
    {
        public string Action { get; set; } = "";
    }
}
