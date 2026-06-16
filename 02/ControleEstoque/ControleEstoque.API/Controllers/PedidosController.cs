﻿using ControleEstoque.API.DTOs;
using ControleEstoque.API.Models;
using ControleEstoque.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;

namespace ControleEstoque.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PedidosController : ControllerBase
    {
        private readonly IPedidoService _pedidoService;

        public PedidosController(IPedidoService pedidoService)
        {
            _pedidoService = pedidoService;
        }
        //Cliente só pode ver o proprio Pedido
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPedido(int id)
        {
            var pedido = await _pedidoService.ObterPedidoComDetalhesAsync(id);

            if (pedido == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Cliente")) 
            {
                var clienteId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (pedido.ClienteId.ToString() != clienteId) 
                {
                    return BadRequest();
                }
            }

            var pedidoDto = new PedidoDto
            {
                Id = pedido.Id,
                DataPedido = pedido.DataPedido,
                Status = pedido.Status,
                ClienteId = pedido.ClienteId,
                Itens = pedido.Itens.Select(i => new ItemPedidoDto
                {
                    Id = i.Id,
                    Quantidade = i.Quantidade,
                    PrecoUnitario = i.PrecoUnitario,
                    ProdutoId = i.ProdutoId,
                    ProdutoNome = i.Produto?.Nome ?? string.Empty
                }).ToList()
            };

            return Ok(pedidoDto);
        }

        [HttpPost]
<<<<<<< HEAD
        [Authorize(Roles = "Cliente")]
        public async Task<IActionResult> CriarPedido([FromBody] CriarPedidoDto pedido)
        {

     
            try
            {
                var clienteIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(clienteIdClaim) || !int.TryParse(clienteIdClaim, out int clienteId))
                {
                    return Unauthorized("Usuário não autenticado ou token inválido.");
                }
=======
        [Authorize(Roles = "Cliente")] //só clientes podem criar pedido
        public async Task<IActionResult> CriarPedido([FromBody] CriarPedidoDto pedido)
        {
            
            try
            {
            
                var clienteIdClaim =int.Parse((User.FindFirst(ClaimTypes.NameIdentifier)?.Value));

>>>>>>> b187db1c3dcaaaf66aaa1b96a32bed1cdca97ac1

                var itensPedido = pedido.Itens.Select(i => new ItemPedido
                {
                    ProdutoId = i.ProdutoId,
                    Quantidade = i.Quantidade
                }).ToList();

<<<<<<< HEAD
                var novoPedido = await _pedidoService.CriarPedidoAsync(clienteId, itensPedido);
=======
                var novoPedido = await _pedidoService.CriarPedidoAsync(clienteIdClaim, itensPedido);
>>>>>>> b187db1c3dcaaaf66aaa1b96a32bed1cdca97ac1
                
                return CreatedAtAction(nameof(GetPedido), new { id = novoPedido.Id }, new 
                { 
                    novoPedido.Id, 
                    novoPedido.Status, 
                    novoPedido.DataPedido 
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
