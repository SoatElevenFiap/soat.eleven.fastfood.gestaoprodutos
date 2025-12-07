using Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Produtos;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Entities;

namespace Soat.Eleven.FastFood.GestaoProdutos.Core.Presenters;

public class ProdutoPresenter
{
    public static ProdutoDto Output(Produto output)
    {
        return new ProdutoDto
        {
            Id = output.Id,
            Nome = output.Nome,
            SKU = output.SKU,
            Descricao = output.Descricao,
            Preco = output.Preco,
            CategoriaId = output.CategoriaId,
            Ativo = output.Ativo,
            CriadoEm = output.CriadoEm,
            Imagem = output.Imagem
        };
    }
}
