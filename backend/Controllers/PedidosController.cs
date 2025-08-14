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

        /// <summary>
        /// Cadastro de Pedidos.
        /// </summary>
        /// <remarks>
        /// Cadastra o pedido no banco de dados, antes conferindo o estoque nos produtos. Se tiver, adiciona novo pedido e retira as quantidade de produtos.
        /// </remarks>
        /// <param name="req">Nome do cliente, e todos os produtos do cliente juntamente com seus quantidades</param>
        /// <returns>Retorna o pedido criado</returns>
        /// <response code="201">Produto Criado com sucesso</response>
        /// <response code="400">Retorna erros de validação de produtos. Com relação a quantidade ou ID do produto</response>
        /// <response code="500">Erro interno do servidor</response>
        [ProducesResponseType(typeof(Pedidos), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
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


        /// <summary>
        /// Lista de Pedidos.
        /// </summary>
        /// <remarks>
        /// Lista todos os pedidos do banco de dados
        /// </remarks>
        /// <returns>Retorna os pedidos do banco</returns>
        /// <response code="200">Lista de Pedidos</response>
        /// <response code="500">Erro interno do servidor</response>
        [ProducesResponseType(typeof(Produtos), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpGet]

        public async Task<ActionResult<Pedidos>> GetPedido()
        {
            var pedidos = await _databaseContext.Pedidos.ToListAsync();
            return Ok(pedidos);
        }

        /// <summary>
        /// Lista de Pedidos.
        /// </summary>
        /// <remarks>
        /// Lista todos os pedidos do banco de dados
        /// </remarks>
        /// <param name="req">Identificador do Pedido</param>
        /// <returns>Retorna os pedidos do banco</returns>
        /// <response code="200">Lista de Produtos</response>
        /// <response code="404">Pedido não encontrado</response>
        /// <response code="500">Erro interno do servidor</response>
        [ProducesResponseType(typeof(Pedidos), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
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