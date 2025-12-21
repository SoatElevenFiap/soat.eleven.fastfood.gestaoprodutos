using Soat.Eleven.FastFood.GestaoProdutos.Core.Entities;

namespace Soat.Eleven.FastFood.GestaoProdutos.Core.Interfaces.Gateways;

public interface ICategoriaProdutoGateway
{
    Task CriarCategoria(CategoriaProduto novaCategoria);
    Task AtualizarCategoria(CategoriaProduto categoria);
    Task<CategoriaProduto?> ObterCategoriaPorId(Guid id);
    Task<IEnumerable<CategoriaProduto>> ListarCategoriasAtivas();
    Task<IEnumerable<CategoriaProduto>> ListarTodasCategorias();
    Task<bool> CategoriaExiste(string nome);
}