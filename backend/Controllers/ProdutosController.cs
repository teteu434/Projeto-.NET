using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SistemaMatheus.Data;
using SistemaMatheus.Models;
using SistemaMatheus.DTO;

namespace SistemaMatheus.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        public ProdutosController(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        /// <summary>
        /// Cadastro de Produtos.
        /// </summary>
        /// <remarks>
        /// Cadastra o produto no Banco de dados.
        /// </remarks>
        /// <param name="produto">Dados de cadastro do usuário</param>
        /// <returns>Retorna o produto criado</returns>
        /// <response code="201">Produto Criado com sucesso</response>
        /// <response code="400">Retorna erros de validação</response>
        /// <response code="500">Erro interno do servidor</response>
        [ProducesResponseType(typeof(Produtos), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        public async Task<IActionResult> PostProduto([FromBody] Produtos produto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _databaseContext.Produtos.Add(produto);
            await _databaseContext.SaveChangesAsync();
            return Created($"/api/produtos/{produto.Id}", produto);
        }


        /// <summary>
        /// Lista de Produtos.
        /// </summary>
        /// <remarks>
        /// Retorna todos os produtos presentes no Banco de dados.
        /// </remarks>
        /// <returns>Retorna uma lista de produtos</returns>
        /// <response code="200">Produto Criado com sucesso</response>
        [ProducesResponseType(typeof(Produtos), StatusCodes.Status200OK)]
        [HttpGet]

        public async Task<ActionResult<Produtos>> GetProduto()
        {
            var produtos = await _databaseContext.Produtos.ToListAsync();
            return Ok(produtos);
        }

        /// <summary>
        /// Busca de Produtos.
        /// </summary>
        /// <remarks>
        /// Busca um produto específico do banco de dados.
        /// </remarks>
        /// <param name="id">Identificador do produto</param>
        /// <returns>Retorna o produto buscado do banco de dados</returns>
        /// <response code="200">Produto foi encontrado com sucesso</response>
        /// <response code="404">Produto buscado não foi encontrado</response>
        /// <response code="500">Erro interno do servidor</response>
        [ProducesResponseType(typeof(Produtos), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpGet("{id}")]

        public async Task<ActionResult<Produtos>> GetProdutoId(int id)
        {
            var produto = await _databaseContext.Produtos.FindAsync(id);
            if (produto == null)
            {
                return NotFound("Produto não encontrado!");
            }
            return Ok(produto);
        }

        /// <summary>
        /// Atualização de Produtos.
        /// </summary>
        /// <remarks>
        /// É escolhido um campo e seu novo valor para ser atualizado em uma row específica do banco de dados.
        /// </remarks>
        /// <param name="id">Identificador do produto</param>
        /// <param name="req">Coluna e o valor que serão alterados</param>
        /// <returns>Retorna o produto novo alterado</returns>
        /// <response code="200">Produto alterado com sucesso</response>
        /// <response code="400">Erros de tipo de valor da coluna</response>
        /// <response code="404">Produto não encontrado</response>
        /// <response code="500">Erro interno do servidor</response>
        [ProducesResponseType(typeof(Produtos), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateProduto(int id, [FromBody] UpdateProdutoRequest req)
        {
            var produto = await _databaseContext.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound("Produto não encontrado!");
            }

            switch (req.Coluna.ToLower())
            {
                case "nome":
                    if (req.Valor.ValueKind != JsonValueKind.String)
                        return BadRequest("O campo 'descrição' deve ser string.");
                    produto.Nome = req.Valor.GetString();
                    break;

                case "preco":
                    if (req.Valor.ValueKind != JsonValueKind.Number)
                        return BadRequest("O campo 'preço' deve ser decimal.");
                    produto.Preco = req.Valor.GetDouble();
                    break;

                case "descrição":
                case "descricao":
                    if (req.Valor.ValueKind != JsonValueKind.String)
                        return BadRequest("O campo 'descrição' deve ser string.");
                    produto.Descricao = req.Valor.GetString();
                    break;

                case "qtd":
                    if (req.Valor.ValueKind != JsonValueKind.Number)
                        return BadRequest("O campo 'quantidade' deve ser um inteiro.");

                    if (req.Valor.TryGetInt32(out int qtdInteira))
                        produto.Qtd = qtdInteira;
                    else
                        return BadRequest("O campo 'quantidade' deve ser um número inteiro (sem decimais).");
                    
                    produto.Qtd = (int)req.Valor.GetInt32();
                    break;

                default:
                    return BadRequest("Coluna inválida.");
            }

            await _databaseContext.SaveChangesAsync();
            return StatusCode(201, produto);
        }

        /// <summary>
        /// Exclusão de Produtos.
        /// </summary>
        /// <remarks>
        /// Exclui um produto do banco de dados.
        /// </remarks>
        /// <param name="id">Identificador do produto</param>
        /// <returns></returns>
        /// <response code="200">Produto Excluído com sucesso</response>
        /// <response code="404">Produto não encontrado</response>
        /// <response code="500">Erro interno do servidor</response>
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            var produto = await _databaseContext.Produtos.FindAsync(id);

            if (produto == null)
            {
                return NotFound("Produto não encontrado!");
            }

            _databaseContext.Produtos.Remove(produto);
            await _databaseContext.SaveChangesAsync();
            return Ok("Deletado com sucesso");            
        }

    }

}