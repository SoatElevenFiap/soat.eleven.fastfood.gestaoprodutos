using Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Categorias;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Entities;

namespace Soat.Eleven.FastFood.GestaoProdutos.Core.Presenters;

public class CategoriaProdutoPresenter
{  
    public static CategoriaProdutoDto Output(CategoriaProduto output)
    {
        return new CategoriaProdutoDto
        {
            Id = output.Id,
            Nome = output.Nome,
            Descricao = output.Descricao,
            Ativo = output.Ativo
        };
    }
}
