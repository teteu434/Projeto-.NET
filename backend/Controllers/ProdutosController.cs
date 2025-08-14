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

        [HttpPost]
        public async Task<IActionResult> PostProduto([FromBody] Produtos produto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _databaseContext.Produtos.Add(produto);
            await _databaseContext.SaveChangesAsync();
            return Created($"/api/produtos/{produto.Id}",produto);
        }

        [HttpGet]

        public async Task<ActionResult<Produtos>> GetProduto()
        {
            var produtos = await _databaseContext.Produtos.ToListAsync();
            return Ok(produtos);
        }

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