using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Pagamentos.Application.Services;
using Pagamentos.Domain.Entities;
using Pagamentos.Infrastructure.Data;
using Processamento.Models;
using System.Text.Json;


namespace Processamento.Services
{
    public class ProcessarPagamentoQueueFunction
    {
        private readonly PagamentosDbContext _context;
        private readonly GatewayPagamentoService _gateway;
        private readonly ILogger<ProcessarPagamentoQueueFunction> _logger;

        public ProcessarPagamentoQueueFunction(
            PagamentosDbContext context,
            GatewayPagamentoService gateway,
            ILogger<ProcessarPagamentoQueueFunction> logger)
        {
            _context = context;
            _gateway = gateway;
            _logger = logger;
        }

        [Function("ProcessarPagamentoQueue")]
        public async Task Run(
            [QueueTrigger("pagamentos-pendentes", Connection = "AzureWebJobsStorage")] string mensagem)
        {
            _logger.LogInformation($"Mensagem recebida da fila: {mensagem}");

            // Desserializa a mensagem da fila em um objeto Pagamento
            var pagamentoFila = JsonSerializer.Deserialize<FilaObject>(mensagem);
            if (pagamentoFila == null)
            {
                _logger.LogError("Não foi possível desserializar o pagamento da fila.");
                return;
            }

            // Busca o pagamento real no banco
            var pagamentoDb = await _context.Pagamentos.FindAsync(pagamentoFila.Id);
            if (pagamentoDb == null)
            {
                _logger.LogError($"Pagamento {pagamentoFila.Id} não encontrado no banco.");
                return;
            }

            // Processa o pagamento usando o Gateway
            await _gateway.ProcessarPagamentoAsync(pagamentoDb);

            _logger.LogInformation($"Pagamento {pagamentoDb.Id} processado com status {pagamentoDb.Status}");
        }
    }
}
