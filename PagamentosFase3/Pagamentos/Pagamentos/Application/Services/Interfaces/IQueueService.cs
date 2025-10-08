namespace Pagamentos.Application.Services.Interfaces
{
    public interface IQueueService
    {
        Task SendMessageAsync(string queueName, object payload);
    }
}
