using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SistemaMatheus.Data;
using SistemaMatheus.Models;
using SistemaMatheus.DTO;
using Microsoft.EntityFrameworkCore;

namespace SistemaMatheus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        public PedidosController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        [HttpPost]
        public async Task<IActionResult> PostPedido([FromBody] CriarPedidoRequest req)
        {
            double valorTotal = 0;

            foreach (var item in req.Itens)
            {
                var produto = await _databaseContext.Produtos.FindAsync(item.IdProduto);

                if (produto == null)
                    return BadRequest($"Produto {item.IdProduto} não encontrado.");

                if (produto.Qtd < item.Qtd)
                    return BadRequest($"Estoque insuficiente para o produto {produto.Nome}.");

                if (produto.Qtd == 0)
                    return BadRequest($"Quantidade do produto {produto.Nome} não pode ser nula.");

                // Calcula o valor total
                valorTotal += produto.Preco * item.Qtd;

                // Atualiza estoque
                produto.Qtd -= item.Qtd;

            }

            var pedido = new Pedidos
            {
                Nome = req.NomeCliente,
                Valor = valorTotal
            };

            _databaseContext.Pedidos.Add(pedido);
            await _databaseContext.SaveChangesAsync();

            return Created($"/api/pedidos/{pedido.Id}", pedido);
        }

        [HttpGet]

        public async Task<ActionResult<Pedidos>> GetPedido()
        {
            var pedidos = await _databaseContext.Pedidos.ToListAsync();
            return Ok(pedidos);
        }
        
        [HttpGet("{id}")]

        public async Task<ActionResult<Pedidos>> GetPedidoId(int id)
        {
            var pedido = await _databaseContext.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound("Pedido não encontrado!");
            }
            return Ok(pedido);
        }
    }
}