using ControleEstoque.API.DTOs;
using ControleEstoque.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ControleEstoque.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContasReceberController : ControllerBase
    {
        private readonly IContaReceberService _contaReceberService;

        public ContasReceberController(IContaReceberService contaReceberService)
        {
            _contaReceberService = contaReceberService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var contas = await _contaReceberService.ObterTodosAsync();
            return Ok(contas);
        }

        // O cliente só deve acessar conta a receber que pertence exclusivamente a ele
        // Há dois caminhos. Restringir esse endpoint para gerente e caixa...
        // ... criando outro caminho que busca a conta a receber por id (e resgta do bearer token)
        // oooou, aqui vc resgata a conta, e verifica se o clienteID da conta é igual ao do bearer token
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {            
            var conta = await _contaReceberService.ObterPorIdAsync(id);
            if (conta == null) return NotFound();

            var usuarioECliente = User.IsInRole("Cliente");
            var usuarioEAdmin = User.IsInRole("Gerente") || User.IsInRole("Caixa");

            if (usuarioECliente) 
            {
                var clienteIdnoToken = User.FindFirst(ClaimTypes.NameIdentifier) ?.Issuer;

                if (conta.ClienteId.ToString() != clienteIdnoToken) 
                {
                    return NoContent();
                }
            }
            return Ok(conta);
        }

        [HttpPost]
        [Authorize(Roles = "Gerente,Caixa")]
        public async Task<IActionResult> Create([FromBody] CriarContaReceberDto dto)
        {
            var novaConta = await _contaReceberService.CriarAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = novaConta.Id }, novaConta);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Gerente,Caixa")]
        public async Task<IActionResult> Update(int id, [FromBody] AtualizarContaReceberDto dto)
        {
            if (id != dto.Id) return BadRequest("O ID da rota difere do ID da conta a receber.");
            
            await _contaReceberService.AtualizarAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Gerente,Caixa")]
        public async Task<IActionResult> Delete(int id)
        {
            await _contaReceberService.RemoverAsync(id);
            return NoContent();
        }
    }
}