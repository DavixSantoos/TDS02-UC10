using ControleEstoque.API.DTOs;
using ControleEstoque.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ControleEstoque.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Qualquer usuário autenticado lê
    public class FormaPagamentoController : ControllerBase
    {
        private readonly IFormaPagamentoService _formaPagamentoService;

        public FormaPagamentoController(IFormaPagamentoService formaPagamentoService)
        {
            _formaPagamentoService = formaPagamentoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var formas = await _formaPagamentoService.ObterTodosAsync();
            return Ok(formas);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var forma = await _formaPagamentoService.ObterPorIdAsync(id);
            if (forma == null) return NotFound("Forma de pagamento não encontrada.");

            return Ok(forma);
        }

        [HttpPost]
        [Authorize(Roles = "Gerente")] // Apenas perfil Gerente cria
        public async Task<IActionResult> Create([FromBody] CriarFormaPagamentoDto dto)
        {
            var novaForma = await _formaPagamentoService.CriarAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = novaForma.Id }, novaForma);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Gerente")] // Apenas perfil Gerente atualiza
        public async Task<IActionResult> Update(int id, [FromBody] AtualizarFormaPagamentoDto dto)
        {
            if (id != dto.Id) return BadRequest("O ID da rota difere do ID informado.");

            await _formaPagamentoService.AtualizarAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Gerente")] // Apenas perfil Gerente deleta
        public async Task<IActionResult> Delete(int id)
        {
            await _formaPagamentoService.RemoverAsync(id);
            return NoContent();
        }
    }
}