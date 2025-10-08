using Azure.Storage.Queues;
using System.Text.Json;

namespace Pagamentos.Application.Services
{
    public class QueueService : Interfaces.IQueueService
    {
        private readonly QueueClient _queueClient;

        public QueueService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AzureStorage");
            _queueClient = new QueueClient(connectionString, "pagamentos-pendentes");
            _queueClient.CreateIfNotExists();
        }

        public async Task SendMessageAsync(string queueName, object payload)
        {
            var json = JsonSerializer.Serialize(payload);
            await _queueClient.SendMessageAsync(Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json)));
        }
    }
}
