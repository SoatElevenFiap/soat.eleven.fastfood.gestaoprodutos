using Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Categorias;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Gateways;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Interfaces.DataSources;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Presenters;
using Soat.Eleven.FastFood.GestaoProdutos.Core.UsesCases;

namespace Soat.Eleven.FastFood.GestaoProdutos.Application.Controllers;

public class CategoriaProdutoController
{
    private readonly ICategoriaProdutoDataSource _categoriaDataSource;

    public CategoriaProdutoController(ICategoriaProdutoDataSource categoriaDataSource)
    {
        _categoriaDataSource = categoriaDataSource;
    }

    private CategoriaProdutoUseCase FabricarUseCase()
    {
        var categoriaGateway = new CategoriaProdutoGateway(_categoriaDataSource);
        return CategoriaProdutoUseCase.Create(categoriaGateway);
    }

    public async Task<IEnumerable<CategoriaProdutoDto>> ListarCategorias(bool incluirInativos)
    {
        var useCase = FabricarUseCase();

        var result = await useCase.ListarCategorias(incluirInativos);

        return result.Select(CategoriaProdutoPresenter.Output);
    }

    public async Task<CategoriaProdutoDto?> GetCategoriaPorId(Guid id)
    {
        var useCase = FabricarUseCase();

        var result = await useCase.ObterCategoriaPorId(id);

        if (result == null)
            return null;

        return CategoriaProdutoPresenter.Output(result);
    }

    public async Task<CategoriaProdutoDto> CriarCategoria(CriarCategoriaDto criarCategoria)
    {
        var useCase = FabricarUseCase();

        var result = await useCase.CriarCategoria(criarCategoria.Nome, criarCategoria.Descricao);

        return CategoriaProdutoPresenter.Output(result);
    }

    public async Task<CategoriaProdutoDto> AtualizarCategoria(Guid id, AtualizarCategoriaDto atualizarCategoria)
    {
        var useCase = FabricarUseCase();

        var result = await useCase.AtualizarCategoria(id, atualizarCategoria.Nome, atualizarCategoria.Descricao);

        return CategoriaProdutoPresenter.Output(result);
    }

    public async Task DesativarCategoria(Guid id)
    {
        var useCase = FabricarUseCase();
        await useCase.DesativarCategoria(id);
    }

    public async Task ReativarCategoria(Guid id)
    {
        var useCase = FabricarUseCase();
        await useCase.ReativarCategoria(id);
    }    
}