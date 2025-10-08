using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pagamentos.Application.DTOs;
using Pagamentos.Application.Services;
using Pagamentos.Application.Services.Interfaces;
using Pagamentos.Domain.Entities;
using Pagamentos.Domain.Enuns;
using Pagamentos.Infrastructure.Data;

namespace Pagamentos.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagamentosController : ControllerBase
    {
        private readonly PagamentosDbContext _context;
        private readonly IQueueService _queueService;

        public PagamentosController(PagamentosDbContext context,  IQueueService queueService)
        {
            _context = context;
            _queueService = queueService;
        }

        // POST api/pagamentos
        [HttpPost]
        public async Task<IActionResult> CriarPagamento([FromBody] CriaPagamentoDto pagamentoDto)
        {

            Pagamento pagamento = new Pagamento
            {
                UsuarioId = pagamentoDto.UsuarioId,
                JogoId = pagamentoDto.JogoId,
                Valor = pagamentoDto.Valor,
                MetodoPagamento = pagamentoDto.MetodoPagamento
            };

            pagamento.Status = StatusPagamento.Pendente;
            pagamento.DataPagamento = DateTime.UtcNow;

            await _context.Pagamentos.AddAsync(pagamento);
            await _context.SaveChangesAsync();

            // Envia para a fila (em vez de processar direto)
            await _queueService.SendMessageAsync("pagamentos-pendentes", new
            {
                pagamento.Id,
                pagamento.UsuarioId,
                pagamento.Valor
            });

            return CreatedAtAction(nameof(ObterPagamentoPorId), new { id = pagamento.Id }, pagamento);
        }

        // GET api/pagamentos/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPagamentoPorId(Guid id)
        {
            var pagamento = await _context.Pagamentos
                .Include(p => p.Transacoes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pagamento == null)
                return NotFound();

            return Ok(pagamento);
        }

        // GET api/pagamentos/usuario/{usuarioId}
        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> ObterPagamentosPorUsuario(Guid usuarioId)
        {
            var pagamentos = await _context.Pagamentos
                .Where(p => p.UsuarioId == usuarioId)
                .ToListAsync();

            return Ok(pagamentos);
        }

        // PUT api/pagamentos/{id}/cancelar
        [HttpPut("{id}/cancelar")]
        public async Task<IActionResult> CancelarPagamento(Guid id)
        {
            var pagamento = await _context.Pagamentos.FindAsync(id);

            if (pagamento == null)
                return NotFound();

            if (pagamento.Status != StatusPagamento.Pendente)
                return BadRequest("Só é possível cancelar pagamentos pendentes.");

            pagamento.Status = StatusPagamento.Cancelado;

            _context.Pagamentos.Update(pagamento);
            await _context.SaveChangesAsync();

            return Ok(pagamento);
        }

        // GET api/pagamentos/{id}/transacoes
        [HttpGet("{id}/transacoes")]
        public async Task<IActionResult> ObterTransacoesPagamento(Guid id)
        {
            var pagamento = await _context.Pagamentos
                .Include(p => p.Transacoes)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pagamento == null)
                return NotFound();

            return Ok(pagamento.Transacoes);
        }
    }
}
