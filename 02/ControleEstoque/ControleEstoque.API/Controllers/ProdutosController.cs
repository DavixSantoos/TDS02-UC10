using ControleEstoque.API.DTOs;
using ControleEstoque.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//qualquer usuario autentencaido podem ver os produtos
//apenas gerentes podem criar, atualizar produtos
namespace ControleEstoque.API.Controllers
{
    [ApiController]
    
    [Route("api/[controller]")]
    [Authorize]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutosController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet]
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> GetAll()
        {
            var produtos = await _produtoService.ObterTodosAsync();
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var produto = await _produtoService.ObterPorIdAsync(id);
            if (produto == null) return NotFound();
            return Ok(produto);
        }

        [HttpPost]
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> Create([FromBody] CriarProdutoDto dto)
        {
            var novoProduto = await _produtoService.CriarAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = novoProduto.Id }, novoProduto);
        }

        [HttpPut("{id}")]
<<<<<<< HEAD
        [Authorize(Roles = "Gerente")]
=======
        [Authorize(Roles ="Gerente")]//
>>>>>>> b187db1c3dcaaaf66aaa1b96a32bed1cdca97ac1
        public async Task<IActionResult> Update(int id, [FromBody] AtualizarProdutoDto dto)
        {
            if (id != dto.Id) return BadRequest("O ID da rota difere do ID do produto.");
            
            await _produtoService.AtualizarAsync(dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> Delete(int id)
        {
            await _produtoService.RemoverAsync(id);
            return NoContent();
        }
    }
}