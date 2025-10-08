using Pagamentos.Domain.Entities;
using Pagamentos.Domain.Enuns;
using Pagamentos.Infrastructure.Data;

namespace Pagamentos.Application.Services
{
    public class GatewayPagamentoService
    {
        private readonly PagamentosDbContext _context;
        private readonly Random _random = new();

        public GatewayPagamentoService(PagamentosDbContext context)
        {
            _context = context;
        }

        public async Task ProcessarPagamentoAsync(Pagamento pagamento)
        {
            // Simula processamento externo
            await Task.Delay(1000); // delay para simular chamada ao gateway

            var sucesso = _random.Next(0, 100) >= 10; // 90% de chance de sucesso

            if (sucesso)
            {
                pagamento.Status = StatusPagamento.Aprovado;

                var transacao = new Transacao
                {
                    PagamentoId = pagamento.Id,
                    MensagemRetorno = "Pagamento aprovado com sucesso",
                };

                await _context.Transacoes.AddAsync(transacao);
            }
            else
            {
                pagamento.Status = StatusPagamento.Cancelado;

                var transacao = new Transacao
                {
                    PagamentoId = pagamento.Id,
                    MensagemRetorno = "Falha no processamento do pagamento",
                };

                await _context.Transacoes.AddAsync(transacao);
            }

            _context.Pagamentos.Update(pagamento);
            await _context.SaveChangesAsync();

        }
    }
}