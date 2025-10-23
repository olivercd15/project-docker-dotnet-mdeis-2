using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Ventas.Domain.Interfaces;
using Azure.Messaging.ServiceBus;

namespace Ventas.Infrastructure.Messaging
{
    public class AzureServiceBusClient : IAzureServiceBusClient
    {
        private readonly string _connectionString;
        private readonly string _responseQueue;

        public AzureServiceBusClient(string connectionString, string responseQueue)
        {
            _connectionString = connectionString;
            _responseQueue = responseQueue;
        }

        public async Task SendMessageAsync(string queueName, object message)
        {
            await using var client = new ServiceBusClient(_connectionString);
            var sender = client.CreateSender(queueName);

            var json = JsonSerializer.Serialize(message);
            var body = new BinaryData(Encoding.UTF8.GetBytes(json));

            await sender.SendMessageAsync(new ServiceBusMessage(body));
        }


        public async Task<string?> ReceiveMessageAsync()
        {
            await using var client = new ServiceBusClient(_connectionString);
            var receiver = client.CreateReceiver(_responseQueue);

            var message = await receiver.ReceiveMessageAsync(TimeSpan.FromSeconds(5));

            if (message == null)
                return null;

            await receiver.CompleteMessageAsync(message);
            return message.Body.ToString();
        }
    }
}
