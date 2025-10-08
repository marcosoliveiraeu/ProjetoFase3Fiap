
using Jogos.Application.Services.Interfaces;
using Jogos.Domain.Entities;
using Jogos.Domain.Exceptions;
using Jogos.Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Jogos.Application.Services
{
    public class JogosUsuarioService : IJogosUsuarioService
    {
        private readonly IJogosUsuarioRepository _jogosUsuarioRepository;
        private readonly IJogoRepository _jogoRepository;

        public JogosUsuarioService(
            IJogosUsuarioRepository jogosUsuarioRepository,
            IJogoRepository jogoRepository)
        {
            _jogosUsuarioRepository = jogosUsuarioRepository;
            _jogoRepository = jogoRepository;
        }

        public async Task AdicionarAsync(Guid usuarioId, Guid jogoId)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(jogoId)
                ?? throw new NotFoundException("Jogo não encontrado.");


            var existe = await _jogosUsuarioRepository.ExisteAsync(usuarioId, jogoId);
            if (existe)
                throw new BusinessException("O usuário já possui este jogo.");


            var jogosUsuario = new JogosUsuario
            {
                Id = Guid.NewGuid(),
                JogoId = jogoId,
                UsuarioId = usuarioId,
                DataAquisicao = DateTime.UtcNow,
                PrecoPago = jogo.Preco
            };

            await _jogosUsuarioRepository.AdicionarAsync(jogosUsuario);
        }

        public async Task<JogosUsuario> ObterPorIdAsync(Guid id)
        {
            var jogoUsuario = await _jogosUsuarioRepository.ObterPorIdAsync(id);
            if (jogoUsuario == null)
                throw new NotFoundException("Registro de jogo do usuário não encontrado.");

            return jogoUsuario;
        }

        public async Task<IEnumerable<JogosUsuario>> ObterPorJogoIdAsync(Guid jogoId)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(jogoId);
            if (jogo == null)
                throw new NotFoundException("Jogo não encontrado.");

            var result =  await _jogosUsuarioRepository.ObterPorJogoIdAsync(jogoId);
            if(result == null)
                throw new NotFoundException("Nenhum usuário está relacionado a esse jogo.");

            return result;
        }



        public async Task<IEnumerable<JogosUsuario>> ObterTodosAsync()
        {
            var jogosUsuarios =  await _jogosUsuarioRepository.ObterTodosAsync();
            if (jogosUsuarios == null)
                throw new NotFoundException("Nenhum registro a ser retornado.");

            return jogosUsuarios;

        }

        public async  Task RemoverAsync(Guid id)
        {
            var entidade = await _jogosUsuarioRepository.ObterPorIdAsync(id);
            if (entidade == null)
                throw new NotFoundException("Registro de jogo do usuário não encontrado.");

            await _jogosUsuarioRepository.RemoverAsync(entidade);
        }
    }

}
