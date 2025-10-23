using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Usuarios.Application.Services;
using System.Threading.Tasks;


namespace Usuarios.Infrastructure.Messaging
{
    public class UsuarioListener
    {
        private readonly ServiceBusClient _client;
        private readonly string _requestQueue;
        private readonly string _responseQueue;
        private readonly UsuarioService _usuarioService;

        public UsuarioListener()
        {
            var connectionString = Environment.GetEnvironmentVariable("AZURE_CONNECTION")!;
            _requestQueue = Environment.GetEnvironmentVariable("USUARIOS_REQUEST_QUEUE")!;
            _responseQueue = Environment.GetEnvironmentVariable("USUARIOS_RESPONSE_QUEUE")!;
            _client = new ServiceBusClient(connectionString);
            _usuarioService = new UsuarioService();
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

                    if (data?.Action == "GetUsuarios")
                    {
                        var usuarios = _usuarioService.GetUsuarios();
                        var json = JsonSerializer.Serialize(usuarios);

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

            Console.WriteLine($"Listener de Usuarios escuchando en '{_requestQueue}'...");
        }
    }


    public class RequestMessage
    {
        public string Action { get; set; } = "";
    }

}
