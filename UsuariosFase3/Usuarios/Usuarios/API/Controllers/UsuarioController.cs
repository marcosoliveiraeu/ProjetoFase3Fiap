﻿

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Usuarios.Application.DTOs;
using Usuarios.Application.Services.Interfaces;
using Usuarios.Domain.Entities;
using Usuarios.Domain.Exceptions;

namespace Usuarios.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : Controller
    {
        private IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }


        /// <summary>
        ///     Cadastrar novo usuário 
        /// </summary>
        /// <remarks>
        ///     Cadastra um novo usuário , esse endPoint é aberto para qualquer novo cliente  
        /// </remarks>
        [HttpPost("Registrar")]
        public async Task<ActionResult> Cadastrar(RegistrarUsuarioDto usuario)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _usuarioService.CadastrarUsuarioAsync(usuario.Nome, usuario.Email, usuario.Senha);
                return Ok(new { message = "Usuário cadastrado com sucesso!" });
            }
            catch (BusinessException ex)
            {
                // Retorna uma mensagem amigável para o usuário
                return BadRequest(new { mensagem = ex.Message });
            }

        }

        /// <summary>
        /// Listar todos os usuários
        /// </summary>
        [HttpGet("BuscarTodos")]
        public async Task<ActionResult<IEnumerable<Usuario>>> ObterTodos()
        {
            var usuarios = await _usuarioService.ObterTodosAsync();

            return Ok(usuarios);
        }

        /// <summary>
        /// Atualizar usuario
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(Guid id, [FromBody] AtualizarUsuarioDto dto)
        {
            if (id != dto.Id)
                return BadRequest("Id da URL diferente do body.");


            await _usuarioService.AtualizarAsync(dto);

            var response = new
            {
                Mensagem = "Usuário atualizado com sucesso.",
                Id = dto.Id,
                Nome = dto.Nome,
                Email = dto.Email,
                Perfil = dto.Perfil
            };


            return Ok(response);
        }

        /// <summary>
        /// Remover usuário
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remover(Guid id)
        {
            var usuario = await _usuarioService.ObterPorIdAsync(id);
            if (usuario == null)
                return NotFound("Usuário não encontrado");

            await _usuarioService.RemoverAsync(id);

            var response = new
            {
                Mensagem = "Usuário excluído com sucesso!",
                Id = usuario.Id,
                Titulo = usuario.Nome,
                Email = usuario.Email,
                Perfil = usuario.Perfil
            };

            return Ok(response);
        }

        /// <summary>
        /// Obter usuário por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> ObterPorId(Guid id)
        {
            var usuario = await _usuarioService.ObterPorIdAsync(id);

            if (usuario == null)
                return NotFound(new { mensagem = "Usuário não encontrado." });

            return Ok(usuario);
        }
    }
}
