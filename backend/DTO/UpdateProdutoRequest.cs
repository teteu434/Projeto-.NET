using System.Text.Json;

namespace SistemaMatheus.DTO
{
    public class UpdateProdutoRequest
    {
        public string Coluna { get; set; }
        public JsonElement Valor { get; set; }
    }
}