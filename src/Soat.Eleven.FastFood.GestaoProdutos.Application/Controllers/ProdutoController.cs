using Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Produtos;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Entities;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Gateways;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Interfaces.DataSources;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Presenters;
using Soat.Eleven.FastFood.GestaoProdutos.Core.UsesCases;

namespace Soat.Eleven.FastFood.GestaoProdutos.Application.Controllers;

public class ProdutoController
{
    private readonly IProdutoDataSource _produtoDataSource;
    private readonly ICategoriaProdutoDataSource _categoriaDataSource;

    public ProdutoController(IProdutoDataSource produtoGateway, ICategoriaProdutoDataSource categoriaProdutoDataSource)
    {
        _produtoDataSource = produtoGateway;
        _categoriaDataSource = categoriaProdutoDataSource;
    }

    private ProdutoUseCase FabricarUseCase()
    {
        var produtoGateway = new ProdutoGateway(_produtoDataSource);
        var categoriaGateway = new CategoriaProdutoGateway(_categoriaDataSource);
        return ProdutoUseCase.Create(produtoGateway, categoriaGateway);
    }

    public async Task<IEnumerable<ProdutoDto>> ListarProdutos(Guid? categoriaId, bool incluirInativos)
    {
        var useCase = FabricarUseCase();

        IEnumerable<Produto> result = await useCase.ListarProdutos(incluirInativos, categoriaId);

        return result.Select(ProdutoPresenter.Output);
    }

    public async Task<ProdutoDto?> GetProduto(Guid id)
    {
        var useCase = FabricarUseCase();
        var result = await useCase.ObterProdutoPorId(id);

        if (result == null)
            return null;

        return ProdutoPresenter.Output(result);
    }

    public async Task<ProdutoDto> CriarProduto(CriarProdutoDto criarProduto)
    {
        var useCase = FabricarUseCase();
        var result = await useCase.CriarProduto(criarProduto);

        return ProdutoPresenter.Output(result!);
    }

    public async Task<ProdutoDto> AtualizarProduto(AtualizarProdutoDto atualizarProduto)
    {
        var useCase = FabricarUseCase();
        var result = await useCase.AtualizarProduto(atualizarProduto);

        return ProdutoPresenter.Output(result);
    }

    public async Task DesativarProduto(Guid id)
    {
        var useCase = FabricarUseCase();
        await useCase.DesativarProduto(id);
    }

    public async Task ReativarProduto(Guid id)
    {
        var useCase = FabricarUseCase();
        await useCase.ReativarProduto(id);
    }
}
