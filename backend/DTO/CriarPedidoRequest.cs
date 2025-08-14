using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaMatheus.DTO
{
    // DTO usado para receber dados de criação de pedido no body da requisição
    public class CriarPedidoRequest
    {
        public string NomeCliente { get; set; } 
        public List<ItemPedidoRequest> Itens { get; set; } // Lista de produtos do pedido
    }

    // DTO auxiliar representando cada item do pedido
    public class ItemPedidoRequest
    {
        public int IdProduto { get; set; } 
        public int Qtd { get; set; }      
    }
}
