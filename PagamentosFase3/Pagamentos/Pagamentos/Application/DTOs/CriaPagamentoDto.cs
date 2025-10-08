using Pagamentos.Domain.Enuns;

namespace Pagamentos.Application.DTOs
{
    public class CriaPagamentoDto
    {
        public Guid UsuarioId { get; set; } // Referência ao MS Usuários
        public Guid JogoId { get; set; }    // Referência ao MS Jogos

        public decimal Valor { get; set; }

        public MetodoPagamento MetodoPagamento { get; set; }
    }
}
