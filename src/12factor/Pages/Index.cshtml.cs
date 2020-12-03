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
        private readonly QueueEndpoint _queue;

        public IndexModel(ILogger<IndexModel> logger, QueueEndpoint queue)
        {
            _logger = logger;
            _queue = queue;
        }

        public async Task OnPostAsync()
        {
            await _queue.SendMessage(Guid.NewGuid().ToString());
        }
    }
}
