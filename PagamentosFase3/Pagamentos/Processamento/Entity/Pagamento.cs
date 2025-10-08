using Pagamentos.Domain.Enuns;

namespace Pagamentos.Domain.Entities
{
    public class Pagamento
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UsuarioId { get; set; } // Referência ao MS Usuários
        public Guid JogoId { get; set; }    // Referência ao MS Jogos

        public decimal Valor { get; set; }
        public DateTime DataPagamento { get; set; } = DateTime.UtcNow;

        public StatusPagamento Status { get; set; } = StatusPagamento.Pendente;
        public MetodoPagamento MetodoPagamento { get; set; }

        public ICollection<Transacao>? Transacoes { get; set; }
    }
}
