

using Jogos.Domain.Enuns;

namespace Jogos.Application.DTOs
{
    public class JogoResponseDto
    {
        public Guid Id { get; set; }
        public string Titulo { get; set; }
        public CategoriaJogo Categoria { get; set; }
        public decimal Preco { get; set; }
        public DateTime DataLancamento { get; set; }
        public DateTime DataAtualizacao { get; set; }

    }

}
