using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Delay(1000, stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                var client = new QueueClient(_configuration["StorageConnectionString"], "12factorqueue");
                await client.CreateIfNotExistsAsync();

                if (await client.ExistsAsync(stoppingToken))
                {
                    // Send a message to the queue
                    var message = await client.ReceiveMessageAsync(TimeSpan.FromSeconds(30), stoppingToken);

                    if(message == null)
                    {
                        await Task.Delay(1000);
                    }
                    else
                    {
                        Console.WriteLine($"Message: {message.Value.Body.ToString()}");
                        await client.DeleteMessageAsync(message.Value.MessageId, message.Value.PopReceipt);
                    }
                }
            }
        }
    }
}
