using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.Data;
using Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.DataSources;
using Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Categorias;
using Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Produtos;
using Xunit;

namespace Soat.Eleven.FastFood.GestaoProdutos.Tests.IntegrationTests.DataSources;

public class ProdutoDataSourceTests : IDisposable
{
    private readonly AppDbContext _context;
    private readonly ProdutoDataSource _dataSource;
    private readonly CategoriaProdutoDataSource _categoriaDataSource;

    public ProdutoDataSourceTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _dataSource = new ProdutoDataSource(_context);
        _categoriaDataSource = new CategoriaProdutoDataSource(_context);
        
        // Ensure database is created
        _context.Database.EnsureCreated();
    }

    [Fact]
    public async Task AddAsync_DeveCriarProduto_ComSucesso()
    {
        // Arrange
        var categoria = await CriarCategoriaAsync();
        var produtoDto = new ProdutoDto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto Teste",
            SKU = "PROD001",
            Descricao = "Descrição do produto",
            Preco = 29.99m,
            CategoriaId = categoria.Id,
            Ativo = true,
            Imagem = "produto.jpg"
        };

        // Act
        await _dataSource.AddAsync(produtoDto);

        // Assert
        var produtoSalvo = await _dataSource.GetByIdAsync(produtoDto.Id);
        produtoSalvo.Should().NotBeNull();
        produtoSalvo!.Nome.Should().Be(produtoDto.Nome);
        produtoSalvo.SKU.Should().Be(produtoDto.SKU);
        produtoSalvo.Preco.Should().Be(produtoDto.Preco);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarProduto_QuandoExistir()
    {
        // Arrange
        var categoria = await CriarCategoriaAsync();
        var produtoDto = new ProdutoDto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto Existente",
            SKU = "PROD002",
            Preco = 19.99m,
            CategoriaId = categoria.Id,
            Ativo = true
        };
        await _dataSource.AddAsync(produtoDto);

        // Act
        var resultado = await _dataSource.GetByIdAsync(produtoDto.Id);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Id.Should().Be(produtoDto.Id);
        resultado.Nome.Should().Be(produtoDto.Nome);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarNull_QuandoNaoExistir()
    {
        // Arrange
        var idInexistente = Guid.NewGuid();

        // Act
        var resultado = await _dataSource.GetByIdAsync(idInexistente);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarTodosProdutos()
    {
        // Arrange
        var categoria = await CriarCategoriaAsync();
        var produto1 = new ProdutoDto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto 1",
            SKU = "PROD003",
            Preco = 10.00m,
            CategoriaId = categoria.Id,
            Ativo = true
        };
        var produto2 = new ProdutoDto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto 2",
            SKU = "PROD004",
            Preco = 20.00m,
            CategoriaId = categoria.Id,
            Ativo = false
        };

        await _dataSource.AddAsync(produto1);
        await _dataSource.AddAsync(produto2);

        // Act
        var resultado = await _dataSource.GetAllAsync();

        // Assert
        resultado.Should().HaveCountGreaterOrEqualTo(2);
        resultado.Should().Contain(p => p.Nome == "Produto 1");
        resultado.Should().Contain(p => p.Nome == "Produto 2");
    }

    [Fact]
    public async Task UpdateAsync_DeveAtualizarProduto_ComSucesso()
    {
        // Arrange
        var categoria = await CriarCategoriaAsync();
        var produtoDto = new ProdutoDto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto Original",
            SKU = "PROD005",
            Preco = 15.00m,
            CategoriaId = categoria.Id,
            Ativo = true
        };
        await _dataSource.AddAsync(produtoDto);

        // Act
        produtoDto.Nome = "Produto Atualizado";
        produtoDto.Preco = 25.00m;
        await _dataSource.UpdateAsync(produtoDto);

        // Assert
        var produtoVerificacao = await _dataSource.GetByIdAsync(produtoDto.Id);
        produtoVerificacao!.Nome.Should().Be("Produto Atualizado");
        produtoVerificacao.Preco.Should().Be(25.00m);
    }

    [Fact]
    public async Task UpdateAsync_DeveLancarExcecao_QuandoProdutoNaoExiste()
    {
        // Arrange
        var categoria = await CriarCategoriaAsync();
        var produtoInexistente = new ProdutoDto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto Inexistente",
            SKU = "PROD999",
            CategoriaId = categoria.Id,
            Ativo = true
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _dataSource.UpdateAsync(produtoInexistente));
        
        exception.Message.Should().Contain($"Produto com Id {produtoInexistente.Id}");
    }

    [Fact]
    public async Task DeleteAsync_DeveDesativarProduto_ComSucesso()
    {
        // Arrange
        var categoria = await CriarCategoriaAsync();
        var produtoDto = new ProdutoDto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto para Deletar",
            SKU = "PROD006",
            Preco = 30.00m,
            CategoriaId = categoria.Id,
            Ativo = true
        };
        await _dataSource.AddAsync(produtoDto);

        // Limpar o context tracking para evitar conflitos
        _context.ChangeTracker.Clear();

        // Obter produto do banco para deletar
        var produtoParaDeletar = await _dataSource.GetByIdAsync(produtoDto.Id);

        // Act
        await _dataSource.DeleteAsync(produtoParaDeletar!);

        // Assert
        var produtoVerificacao = await _dataSource.GetByIdAsync(produtoDto.Id);
        produtoVerificacao.Should().NotBeNull();
        produtoVerificacao!.Ativo.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteAsync_NaoDeveFazerNada_QuandoProdutoNaoExiste()
    {
        // Arrange
        var categoria = await CriarCategoriaAsync();
        var produtoInexistente = new ProdutoDto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto Inexistente",
            SKU = "PROD999",
            CategoriaId = categoria.Id,
            Ativo = true
        };

        // Act - não deve lançar exceção
        await _dataSource.DeleteAsync(produtoInexistente);
        
        // Assert - produto continua não existindo
        var resultado = await _dataSource.GetByIdAsync(produtoInexistente.Id);
        resultado.Should().BeNull();
    }

    [Fact]
    public async Task FindAsync_DeveFiltrarProdutos_CorretamenteComPredicate()
    {
        // Arrange
        var categoria = await CriarCategoriaAsync();
        var produto1 = new ProdutoDto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto Ativo",
            SKU = "PROD007",
            Preco = 40.00m,
            CategoriaId = categoria.Id,
            Ativo = true
        };
        var produto2 = new ProdutoDto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto Inativo",
            SKU = "PROD008",
            Preco = 50.00m,
            CategoriaId = categoria.Id,
            Ativo = false
        };

        await _dataSource.AddAsync(produto1);
        await _dataSource.AddAsync(produto2);

        // Act
        var produtosAtivos = await _dataSource.FindAsync(p => p.Ativo);

        // Assert
        produtosAtivos.Should().Contain(p => p.Nome == "Produto Ativo");
        produtosAtivos.Should().NotContain(p => p.Nome == "Produto Inativo");
    }

    private async Task<CategoriaProdutoDto> CriarCategoriaAsync()
    {
        var categoriaDto = new CategoriaProdutoDto
        {
            Id = Guid.NewGuid(),
            Nome = "Categoria Teste",
            Descricao = "Descrição da categoria",
            Ativo = true
        };

        await _categoriaDataSource.AddAsync(categoriaDto);
        return categoriaDto;
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}