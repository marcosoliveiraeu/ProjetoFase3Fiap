

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Usuarios.Application.DTOs;
using Usuarios.Application.Services.Interfaces;
using Usuarios.Infrastructure.Repository.Interfaces;

namespace Usuarios.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ISenhaHasher _senhaHasher;
        private readonly IConfiguration _configuration;

        public LoginController(IUsuarioRepository usuarioRepository, ISenhaHasher senhaHasher, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _senhaHasher = senhaHasher;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(request.Email);

            if (usuario == null || usuario.SenhaHash != _senhaHasher.Hash(request.Senha)) 
            {
                return Unauthorized("Usuário ou senha inválidos.");
            }

            return Ok();
        }



    }

}
