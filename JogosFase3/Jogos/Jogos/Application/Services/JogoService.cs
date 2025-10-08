
using Jogos.Application.DTOs;
using Jogos.Application.Services.Interfaces;
using Jogos.Domain.Entities;
using Jogos.Domain.Enuns;
using Jogos.Domain.Exceptions;
using Jogos.Infrastructure.Repository.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Globalization;

namespace Jogos.Application.Services
{
    public class JogoService : IJogoService
    {

        private IJogoRepository _jogoRepository;

        public JogoService( IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }

        public async Task<IEnumerable<JogoResponseDto>> ObterTodosAsync()
        {

            var jogos = await _jogoRepository.ObterTodosAsync();

            var response = jogos.Select(jogo =>
            {

                return new JogoResponseDto
                {
                    Id = jogo.Id,
                    Titulo = jogo.Titulo,
                    Categoria = jogo.Categoria,
                    Preco = jogo.Preco,
                    DataLancamento = jogo.DataLancamento,
                    DataAtualizacao = jogo.DataAtualizacao,

                };
            });

            return response;
        }

        public async Task<JogoResponseDto?> ObterPorIdAsync(Guid id)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(id);

            if (jogo == null)
                return null;

            return new JogoResponseDto
            {
                Id = jogo.Id,
                Titulo = jogo.Titulo,
                Categoria = jogo.Categoria,
                Preco = jogo.Preco,
                DataLancamento = jogo.DataLancamento,
                DataAtualizacao = jogo.DataAtualizacao
            };
        }

        public async Task<JogoResponseDto?> ObterPorTituloAsync(string titulo)
        {
            var jogo = await _jogoRepository.ObterPorTituloAsync(titulo);
            if (jogo == null)
                return null;


            return new JogoResponseDto
            {
                Id = jogo.Id,
                Titulo = jogo.Titulo,
                Categoria = jogo.Categoria,
                Preco = jogo.Preco,
                DataLancamento = jogo.DataLancamento,
                DataAtualizacao = jogo.DataAtualizacao
            };


        }

        public async Task<Jogo> AdicionarAsync(CriarJogoDto jogoDto)
        {
            // Verificar se já existe um jogo com esse título
            bool existe = await _jogoRepository.ExistePorTituloAsync(jogoDto.Titulo);
            if (existe)
            {
                throw new BusinessException("Já existe um jogo com esse título.");
            }

            var novoJogo = new Jogo
            {
                Id = Guid.NewGuid(),
                Titulo = jogoDto.Titulo,
                Categoria = jogoDto.Categoria,
                Preco = jogoDto.Preco,
                DataLancamento = jogoDto.DataLancamento,
                DataAtualizacao = DateTime.UtcNow
            };

            await _jogoRepository.AdicionarAsync(novoJogo);

            if (jogoDto.EnviarNotificacao)
            {
                //var todosUsuarios = await _usuarioRepository.ObterTodosAsync();

                //var assunto = $"O jogo {novoJogo.Titulo} chegou!";
                //var mensagem = $"Aproveite! O jogo <b>{novoJogo.Titulo}</b> está em a venda por R$ {novoJogo.Preco:F2}.";

                //foreach (var usuario in todosUsuarios)
                //{
                    //await _emailService.EnviarEmailAsync(usuario.Email, assunto, mensagem);
               // }
            }

            return novoJogo;
        }

        public async Task AtualizarAsync(System.Guid id,string titulo,CategoriaJogo categoria, decimal preco , DateTime dataLancamento)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(id);
            if (jogo == null)
            {
                throw new NotFoundException("Jogo não encontrado.");
            }

            var existeTitulo = await _jogoRepository.ExistePorTituloAsync(jogo.Titulo, jogo.Id);
            if (existeTitulo)
                throw new BusinessException("Já existe outro jogo com este título.");


            jogo.Titulo = titulo;
            jogo.Categoria = categoria;
            jogo.Preco = preco;
            jogo.DataLancamento = dataLancamento;
            jogo.DataAtualizacao = DateTime.UtcNow;

            
            await _jogoRepository.AtualizarAsync(jogo);
        }

        public async Task RemoverAsync(Guid id)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(id);
            if (jogo == null)
            {
                throw new NotFoundException("Jogo não encontrado.");
            }

            await _jogoRepository.RemoverAsync(id);
        }

    }
}
