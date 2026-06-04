using ControleEstoque.API.DTOs;
using ControleEstoque.API.Models;
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

        [HttpGet] //Gerente e caixa
        [Authorize(Roles = "Gerente,Caixa")]
        public async Task<IActionResult> GetAll()
        {

            var contas = await _contaReceberService.ObterTodosAsync();
            return Ok(contas);

        }

        [HttpPost]//Gerente 
        [Authorize(Roles ="Gerente")]
        public async Task<IActionResult> Create([FromBody] CriarContaReceberDto dto)
        {
            var novaConta = await _contaReceberService.CriarAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = novaConta.Id }, novaConta);
        }

        [HttpPut("{id}")] //Gerente e caixa
       
        public async Task<IActionResult> Update(int id, [FromBody] AtualizarContaReceberDto dto)
        {
            if (id != dto.Id) return BadRequest("O ID da rota difere do ID da conta a receber.");
            
            await _contaReceberService.AtualizarAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _contaReceberService.RemoverAsync(id);
            return NoContent();
        }
    }
}