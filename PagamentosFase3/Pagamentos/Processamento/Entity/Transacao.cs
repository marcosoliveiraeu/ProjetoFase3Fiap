using System.Text.Json.Serialization;

namespace Pagamentos.Domain.Entities
{
    public class Transacao
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PagamentoId { get; set; }

        public string CodigoTransacao { get; set; } = Guid.NewGuid().ToString("N");
        public string MensagemRetorno { get; set; } = "Pagamento processado com sucesso";
        public DateTime DataProcessamento { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public Pagamento? Pagamento { get; set; }
    }
}
