using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace _12factor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;

        public int MessageCount {get;set;} = 0;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task OnGetAsync()
        {
            var client = new QueueClient(_configuration["StorageConnectionString"], "12factorqueue");
            await client.CreateIfNotExistsAsync();

            if (await client.ExistsAsync())
            {
                // Retrieve the cached approximate message count.
                QueueProperties properties = await client.GetPropertiesAsync();
                MessageCount = properties.ApproximateMessagesCount;
            }
        }

        public async Task OnPostAsync()
        {
            var id = Guid.NewGuid();

            var client = new QueueClient(_configuration["StorageConnectionString"], "12factorqueue");
            await client.CreateIfNotExistsAsync();

            if (await client.ExistsAsync())
            {
                // Send a message to the queue
                await client.SendMessageAsync(id.ToString());

                // Retrieve the cached approximate message count.
                QueueProperties properties = await client.GetPropertiesAsync();
                MessageCount = properties.ApproximateMessagesCount;
            }
        }
    }
}
