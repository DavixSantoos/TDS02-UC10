using ControleEstoque.API.DTOs;
using ControleEstoque.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [Authorize(Roles = "Gerente")]
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
        [Authorize(Roles = "Gerente")]
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
        //Clliente e caixa só pode atualizar o próprio cadastros
        [HttpPut("atualizar-cliente")]
        [Authorize(Roles = "Caixa")]
        public async Task<IActionResult> AtualizarCliente([FromBody] AtualizarClienteDto dto)
        {
            if (!User.IsInRole("Gerente"))
            {
                var usuarioIdNoToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (dto.Id.ToString() != usuarioIdNoToken) 
                {
                    return NoContent();
                }
            }

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
        [Authorize(Roles = "Gerente")]
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
        [Authorize(Roles = "Gerente")]
        public async Task<IActionResult> AtualizarGerente([FromBody] AtualizarGerenteDto dto)
        {


            if (!User.IsInRole("Gerente"))
            {
                var usuarioIdNoToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (dto.Id.ToString() != usuarioIdNoToken) 
                {
                    return NoContent();
                }
            }
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
        [Authorize(Roles = "Gerente, Caixa")]
        public async Task<IActionResult> GetAll()
        {
            var usuarios = await _usuarioService.ListarTodosUsuariosAsync();
            return Ok(usuarios);
        }
        //Se for o dele, só pode obter dele mesmo

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> ObterPorId(int id)
        {
            if (!User.IsInRole("Gerente") && !User.IsInRole("Caixa")) 
            {
                var usuarioIdNoToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (id.ToString() != usuarioIdNoToken) 
                {
                    return NoContent();
                }
            }

            var usuario = await _usuarioService.ObterUsuarioPorIdAsync(id);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        //Se for o dele, só pode obter dele mesmo
        [HttpGet("email/{email}")]
        [Authorize]
        public async Task<IActionResult> ObterPorEmail(string email)
        {
            if (!User.IsInRole("Gerente") && !User.IsInRole("Caixa")) 
            {
                var emailNoToken = User.FindFirst(ClaimTypes.Email)?.Value;
                if (!string.Equals(email, emailNoToken, StringComparison.OrdinalIgnoreCase)) 
                { 
                    return BadRequest();
                }
            }

            var usuario = await _usuarioService.ObterUsuarioPorEmailAsync(email);
            if (usuario == null) return NotFound();
            return Ok(usuario);
        }

        #endregion

        #region Deleção

        [HttpDelete("{id}")]
        [Authorize(Roles = "Gerente")]
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
                var resultado = await _usuarioService.AutenticarAsync(dto);
                if (resultado == null)
                    return Unauthorized(new { message = "Email ou senha incorretos." });

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        #endregion
    }
}