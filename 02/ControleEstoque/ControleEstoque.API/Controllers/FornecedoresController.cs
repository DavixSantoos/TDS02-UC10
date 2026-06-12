using ControleEstoque.API.DTOs;
using ControleEstoque.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


//só Gerente podem tomar açoes
namespace ControleEstoque.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
<<<<<<< HEAD
    [Authorize(Roles = "Gerente")]
=======
    [Authorize(Roles ="Gerente")]
>>>>>>> b187db1c3dcaaaf66aaa1b96a32bed1cdca97ac1
    public class FornecedoresController : ControllerBase
    {
        private readonly IFornecedorService _fornecedorService;

        public FornecedoresController(IFornecedorService fornecedorService)
        {
            _fornecedorService = fornecedorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var fornecedores = await _fornecedorService.ObterTodosAsync();
            return Ok(fornecedores);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var fornecedor = await _fornecedorService.ObterPorIdAsync(id);
            if (fornecedor == null) return NotFound();
            return Ok(fornecedor);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CriarFornecedorDto dto)
        {
            var novoFornecedor = await _fornecedorService.CriarAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = novoFornecedor.Id }, novoFornecedor);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AtualizarFornecedorDto dto)
        {
            if (id != dto.Id) return BadRequest("O ID da rota difere do ID do fornecedor.");
            
            await _fornecedorService.AtualizarAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _fornecedorService.RemoverAsync(id);
            return NoContent();
        }
    }
}