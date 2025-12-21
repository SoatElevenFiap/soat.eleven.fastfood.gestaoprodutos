using System.Text.Json;
using System.Text.Json.Serialization;

namespace Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Produtos
{
    public class AtualizarProdutoDto
    {
        public Guid Id { get; set; }
        public string SKU { get; set; } = null!;
        public string Nome { get; set; } = null!;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public string? Imagem { get; set; }
        public Guid CategoriaId { get; set; }

        [JsonExtensionData]
        public Dictionary<string, JsonElement>? CamposExtras { get; set; }

        public bool ImagemFoiEnviada()
        {
            return CamposExtras?.ContainsKey(nameof(Imagem)) == true;
        }       
    }
}