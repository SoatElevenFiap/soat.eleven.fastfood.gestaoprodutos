using Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.DataSources;
using Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Categorias;
using Soat.Eleven.FastFood.GestaoProdutos.Tests.Helpers;

namespace Soat.Eleven.FastFood.GestaoProdutos.Tests.IntegrationTests.DataSources;

public class CategoriaProdutoDataSourceTests : IDisposable
{
    private readonly TestDbContextHelper _dbHelper;
    private readonly CategoriaProdutoDataSource _dataSource;

    public CategoriaProdutoDataSourceTests()
    {
        _dbHelper = new TestDbContextHelper();
        _dataSource = new CategoriaProdutoDataSource(_dbHelper.GetContext());
    }

    [Fact]
    public async Task AddAsync_DeveCriarCategoria_ComSucesso()
    {
        // Arrange
        var dto = new CategoriaProdutoDto
        {
            Id = Guid.NewGuid(),
            Nome = "Bebidas",
            Descricao = "Categoria para bebidas",
            Ativo = true
        };

        // Act
        var resultado = await _dataSource.AddAsync(dto);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Id.Should().Be(dto.Id);
        resultado.Nome.Should().Be(dto.Nome);
        resultado.Descricao.Should().Be(dto.Descricao);
        resultado.Ativo.Should().Be(dto.Ativo);

        // Verificar se foi salvo no banco
        var categoriaSalva = await _dataSource.GetByIdAsync(dto.Id);
        categoriaSalva.Should().NotBeNull();
        categoriaSalva!.Nome.Should().Be(dto.Nome);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarCategoria_QuandoExiste()
    {
        // Arrange
        var dto = new CategoriaProdutoDto
        {
            Id = Guid.NewGuid(),
            Nome = "Lanches",
            Descricao = "Categoria para lanches",
            Ativo = true
        };
        await _dataSource.AddAsync(dto);

        // Act
        var resultado = await _dataSource.GetByIdAsync(dto.Id);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Id.Should().Be(dto.Id);
        resultado.Nome.Should().Be(dto.Nome);
        resultado.Descricao.Should().Be(dto.Descricao);
        resultado.Ativo.Should().Be(dto.Ativo);
    }

    [Fact]
    public async Task GetByIdAsync_DeveRetornarNull_QuandoNaoExiste()
    {
        // Arrange
        var idInexistente = Guid.NewGuid();

        // Act
        var resultado = await _dataSource.GetByIdAsync(idInexistente);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_DeveRetornarTodasCategorias()
    {
        // Arrange
        _dbHelper.ClearDatabase();
        
        var categorias = new List<CategoriaProdutoDto>
        {
            new() { Id = Guid.NewGuid(), Nome = "Bebidas", Descricao = "Bebidas", Ativo = true },
            new() { Id = Guid.NewGuid(), Nome = "Lanches", Descricao = "Lanches", Ativo = true },
            new() { Id = Guid.NewGuid(), Nome = "Sobremesas", Descricao = "Sobremesas", Ativo = false }
        };

        foreach (var categoria in categorias)
        {
            await _dataSource.AddAsync(categoria);
        }

        // Act
        var resultado = await _dataSource.GetAllAsync();

        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(3);
        resultado.Should().Contain(c => c.Nome == "Bebidas");
        resultado.Should().Contain(c => c.Nome == "Lanches");
        resultado.Should().Contain(c => c.Nome == "Sobremesas");
    }

    [Fact]
    public async Task FindAsync_DeveRetornarCategoriasAtivas_QuandoFiltroEAplicado()
    {
        // Arrange
        _dbHelper.ClearDatabase();
        
        var categorias = new List<CategoriaProdutoDto>
        {
            new() { Id = Guid.NewGuid(), Nome = "Bebidas", Descricao = "Bebidas", Ativo = true },
            new() { Id = Guid.NewGuid(), Nome = "Lanches", Descricao = "Lanches", Ativo = true },
            new() { Id = Guid.NewGuid(), Nome = "Sobremesas", Descricao = "Sobremesas", Ativo = false }
        };

        foreach (var categoria in categorias)
        {
            await _dataSource.AddAsync(categoria);
        }

        // Act
        var resultado = await _dataSource.FindAsync(c => c.Ativo);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(2);
        resultado.All(c => c.Ativo).Should().BeTrue();
        resultado.Should().Contain(c => c.Nome == "Bebidas");
        resultado.Should().Contain(c => c.Nome == "Lanches");
        resultado.Should().NotContain(c => c.Nome == "Sobremesas");
    }

    [Fact]
    public async Task UpdateAsync_DeveAtualizarCategoria_ComSucesso()
    {
        // Arrange
        var dto = new CategoriaProdutoDto
        {
            Id = Guid.NewGuid(),
            Nome = "Nome Original",
            Descricao = "Descrição Original",
            Ativo = true
        };
        await _dataSource.AddAsync(dto);

        // Modificar os dados
        dto.Nome = "Nome Atualizado";
        dto.Descricao = "Descrição Atualizada";
        dto.Ativo = false;

        // Act
        var resultado = await _dataSource.UpdateAsync(dto);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Id.Should().Be(dto.Id);
        resultado.Nome.Should().Be("Nome Atualizado");
        resultado.Descricao.Should().Be("Descrição Atualizada");
        resultado.Ativo.Should().BeFalse();

        // Verificar se foi atualizado no banco
        var categoriaAtualizada = await _dataSource.GetByIdAsync(dto.Id);
        categoriaAtualizada!.Nome.Should().Be("Nome Atualizado");
        categoriaAtualizada.Descricao.Should().Be("Descrição Atualizada");
        categoriaAtualizada.Ativo.Should().BeFalse();
    }

    [Fact]
    public async Task UpdateAsync_DeveLancarExcecao_QuandoCategoriaNaoExiste()
    {
        // Arrange
        var dto = new CategoriaProdutoDto
        {
            Id = Guid.NewGuid(),
            Nome = "Categoria Inexistente",
            Descricao = "Descrição",
            Ativo = true
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _dataSource.UpdateAsync(dto));
        
        exception.Message.Should().Contain($"Categoria com Id {dto.Id} não encontrada");
    }

    [Fact]
    public async Task DeleteAsync_DeveRemoverCategoria_ComSucesso()
    {
        // Arrange
        var dto = new CategoriaProdutoDto
        {
            Id = Guid.NewGuid(),
            Nome = "Categoria para Deletar",
            Descricao = "Será deletada",
            Ativo = true
        };
        await _dataSource.AddAsync(dto);

        // Verificar que existe
        var categoriaExistente = await _dataSource.GetByIdAsync(dto.Id);
        categoriaExistente.Should().NotBeNull();

        // Act
        await _dataSource.DeleteAsync(dto);

        // Assert
        var categoriaRemovida = await _dataSource.GetByIdAsync(dto.Id);
        categoriaRemovida.Should().BeNull();
    }

    public void Dispose()
    {
        _dbHelper?.Dispose();
        GC.SuppressFinalize(this);
    }
}