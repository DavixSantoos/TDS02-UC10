using ControleEstoque.API.DTOs;
using ControleEstoque.API.Services;
using Microsoft.AspNetCore.Mvc;
//login e aberto a todos cadastro precisa estar autenticado como gerente
namespace ControleEstoque.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        #region Registro

        [HttpPost("registrar-cliente")]
        public async Task<IActionResult> RegistrarCliente([FromBody] CriarClienteDto dto)
        {
            try
            {
                var novoCliente = await _usuarioService.RegistrarClienteAsync(dto);
                return CreatedAtAction(nameof(ObterPorId), new { id = novoCliente.Id }, novoCliente);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("registrar-caixa")]
        public async Task<IActionResult> RegistrarCaixa([FromBody] CriarCaixaDto dto)
        {
            try
            {
                var novoCaixa = await _usuarioService.RegistrarCaixaAsync(dto);
                return CreatedAtAction(nameof(ObterPorId), new { id = novoCaixa.Id }, novoCaixa);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("registrar-gerente")]
        public async Task<IActionResult> RegistrarGerente([FromBody] CriarGerenteDto dto)
        {
            try
            {
                var novoGerente = await _usuarioService.RegistrarGerenteAsync(dto);
                return CreatedAtAction(nameof(ObterPorId), new { id = novoGerente.Id }, novoGerente);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion

        #region Atualização

        [HttpPut("atualizar-cliente")]
        public async Task<IActionResult> AtualizarCliente([FromBody] AtualizarClienteDto dto)
        {
            try
            {
                await _usuarioService.AtualizarClienteAsync(dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("atualizar-caixa")]
        public async Task<IActionResult> AtualizarCaixa([FromBody] AtualizarCaixaDto dto)
        {
            try
            {
                await _usuarioService.AtualizarCaixaAsync(dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("atualizar-gerente")]
        public async Task<IActionResult> AtualizarGerente([FromBody] AtualizarGerenteDto dto)
        {
            try
            {
                await _usuarioService.AtualizarGerenteAsync(dto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion

        #region Consulta

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuarioService.ListarTodosUsuariosAsync();
            return Ok(usuarios);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var usuario = await _usuarioService.ObterUsuarioPorIdAsync(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        [HttpGet("email/{email}")]
        public async Task<IActionResult> ObterPorEmail(string email)
        {
            var usuario = await _usuarioService.ObterUsuarioPorEmailAsync(email);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        #endregion

        #region Deleção

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _usuarioService.RemoverUsuarioAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        #endregion

        #region Autenticação

        [HttpPost("autenticar")]
        public async Task<IActionResult> Autenticar([FromBody] LoginDto dto)
        {
            try
            {
                var usuario = await _usuarioService.AutenticarAsync(dto);
                if (usuario == null)
                    return Unauthorized(new { message = "Email ou senha incorretos." });

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion
    }
}