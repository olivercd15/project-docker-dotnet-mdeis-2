using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ventas.Domain.Interfaces
{
    public interface IAzureServiceBusClient
    {
        Task SendMessageAsync(string queueName, object message);
        Task<string?> ReceiveMessageAsync();
    }
}
