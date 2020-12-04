

using System.Threading.Tasks;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.Extensions.Configuration;

namespace _12factor
{
    public class QueueEndpoint : IMessageEndpoint
    {
        private IConfiguration _configuration;

        public QueueEndpoint(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMessage(string message)
        {
            var client = new QueueClient(_configuration["StorageConnectionString"], _configuration["QueueName"]);
            await client.CreateIfNotExistsAsync();

            if (await client.ExistsAsync())
            {
                // Send a message to the queue
                await client.SendMessageAsync(message);
            }
        }
    }

    public interface IMessageEndpoint
    {
        Task SendMessage(string message);
    }
}