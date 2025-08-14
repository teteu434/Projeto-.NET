using System.Text.Json;

namespace SistemaMatheus.DTO
{
    // DTO usado para requisições de atualização parcial de produto
    public class UpdateProdutoRequest
    {
        public string Coluna { get; set; } // Nome da coluna/campo que será atualizado
        public JsonElement Valor { get; set; } // Valor a ser aplicado na atualização
    }
}
