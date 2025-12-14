using Soat.Eleven.FastFood.GestaoProdutos.Core.Entities;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Interfaces.Gateways;

namespace Soat.Eleven.FastFood.GestaoProdutos.Core.UsesCases;

public class CategoriaProdutoUseCase
{
    private readonly ICategoriaProdutoGateway _categoriaGateway;

    private CategoriaProdutoUseCase(ICategoriaProdutoGateway categoriaGateway)
    {
        _categoriaGateway = categoriaGateway;
    }

    public static CategoriaProdutoUseCase Create(ICategoriaProdutoGateway categoriaGateway)
    {
        return new CategoriaProdutoUseCase(categoriaGateway);
    }

    public async Task<IEnumerable<CategoriaProduto>> ListarCategorias(bool? incluirInativos = false)
    {
        var categorias = incluirInativos == true ?
            await _categoriaGateway.ListarTodasCategorias() :
            await _categoriaGateway.ListarCategoriasAtivas();

        return categorias;
    }

    public async Task<CategoriaProduto?> ObterCategoriaPorId(Guid id)
    {
        var categoria = await _categoriaGateway.ObterCategoriaPorId(id);

        return categoria;
    }

    public async Task<CategoriaProduto> CriarCategoria(string nome, string descricao)
    {
        var categoriaExiste = await _categoriaGateway.CategoriaExiste(nome);

        if (categoriaExiste)
            throw new ArgumentException("Categoria de mesmo nome já existe");

        var novaCategoria = new CategoriaProduto
        {
            Id = Guid.NewGuid(),
            Nome = nome,
            Descricao = descricao,
            Ativo = true
        };

        await _categoriaGateway.CriarCategoria(novaCategoria);

        return novaCategoria;
    }

    public async Task<CategoriaProduto> AtualizarCategoria(Guid id, string nome, string descricao)
    {
        var categoriaExistente = await _categoriaGateway.ObterCategoriaPorId(id);

        if (categoriaExistente == null)
            throw new KeyNotFoundException("Categoria não encontrada");

        categoriaExistente.Nome = nome;
        categoriaExistente.Descricao = descricao;

        await _categoriaGateway.AtualizarCategoria(categoriaExistente);

        return categoriaExistente;
    }

    public async Task DesativarCategoria(Guid id)
    {
        var categoria = await _categoriaGateway.ObterCategoriaPorId(id);

        if (categoria == null)
            throw new KeyNotFoundException("Categoria não encontrada");

        categoria.Ativo = false;
        await _categoriaGateway.AtualizarCategoria(categoria);
    }

    public async Task ReativarCategoria(Guid id)
    {
        var categoria = await _categoriaGateway.ObterCategoriaPorId(id);

        if (categoria == null)
            throw new KeyNotFoundException("Categoria não encontrada");

        categoria.Ativo = true;
        await _categoriaGateway.AtualizarCategoria(categoria);
    }
}
