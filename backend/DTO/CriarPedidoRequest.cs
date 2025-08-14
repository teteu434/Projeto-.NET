using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaMatheus.DTO
{
    public class CriarPedidoRequest
    {
        public string NomeCliente { get; set; }
        public List<ItemPedidoRequest> Itens { get; set; }
    }

    public class ItemPedidoRequest
    {
        public int IdProduto { get; set; }
        public int Qtd { get; set; }
    }
}