using Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.EntityModel.Base;

namespace Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.EntityModel
{
    public class ProdutoModel : EntityBase
    {
        public string Nome { get; set; } = null!;
        public string SKU { get; set; } = null!;
        public string? Descricao { get; set; }
        public decimal Preco { get; set; }
        public Guid CategoriaId { get; set; }
        public bool Ativo { get; set; }
        public string? Imagem { get; set; }
        public CategoriaProdutoModel Categoria { get; set; } = null!;
    }
}