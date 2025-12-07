using Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.EntityModel.Base;

namespace Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.EntityModel
{
    public class CategoriaProdutoModel : EntityBase
    {
        public string Nome { get; set; } = null!;
        public string? Descricao { get; set; }
        public bool Ativo { get; set; } = true;

        public ICollection<ProdutoModel> Produtos { get; set; } = new List<ProdutoModel>();
    }
}