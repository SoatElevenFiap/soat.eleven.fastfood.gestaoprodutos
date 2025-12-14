using Soat.Eleven.FastFood.GestaoProdutos.Core.Entities;

namespace Soat.Eleven.FastFood.GestaoProdutos.Core.Interfaces.Gateways;

public interface IProdutoGateway
{
    Task<IEnumerable<Produto>> ListarTodosProdutos();
    Task<IEnumerable<Produto>> ListarProdutosAtivos();
    Task<IEnumerable<Produto>> ListarProdutosPorCategoria(Guid categoriaId);
    Task<IEnumerable<Produto>> ListarProdutosAtivosPorCategoria(Guid categoriaId);
    Task<Produto?> ObterProdutoPorId(Guid id);
    Task<bool> ProdutoExiste(string sku);
    Task CriarProduto(Produto produto);
    Task AtualizarProduto(Produto produto);
}