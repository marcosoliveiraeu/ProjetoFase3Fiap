

using Jogos.Application.DTOs;
using Jogos.Domain.Entities;
using Jogos.Domain.Enuns;

namespace Jogos.Application.Services.Interfaces
{
    public interface IJogoService
    {
        Task<IEnumerable<JogoResponseDto>> ObterTodosAsync();
        Task<JogoResponseDto?> ObterPorIdAsync(Guid id);
        Task<JogoResponseDto?> ObterPorTituloAsync(string titulo);
        Task<Jogo> AdicionarAsync(CriarJogoDto jogo);
        Task AtualizarAsync(System.Guid id, string titulo, CategoriaJogo categoria, decimal preco, DateTime dataLancamento);
        Task RemoverAsync(Guid id);

    }
}
