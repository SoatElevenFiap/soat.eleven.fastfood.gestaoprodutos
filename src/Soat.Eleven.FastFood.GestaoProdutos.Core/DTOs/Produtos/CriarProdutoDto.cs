namespace Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Produtos
{
    public class CriarProdutoDto
    {
        public string Nome { get; set; } = null!;
        public string SKU { get; set; } = null!;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public string? Imagem { get; set; }
        public Guid CategoriaId { get; set; }     
    }
}