using FluentAssertions;
using NSubstitute;
using Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Produtos;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Entities;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Interfaces.Gateways;
using Soat.Eleven.FastFood.GestaoProdutos.Core.UsesCases;
using Xunit;

namespace Soat.Eleven.FastFood.GestaoProdutos.Tests.UnitTests.Core.UsesCases;

public class ProdutoUseCaseTests
{
    private readonly IProdutoGateway _produtoGateway;
    private readonly ICategoriaProdutoGateway _categoriaGateway;
    private readonly ProdutoUseCase _produtoUseCase;

    public ProdutoUseCaseTests()
    {
        _produtoGateway = Substitute.For<IProdutoGateway>();
        _categoriaGateway = Substitute.For<ICategoriaProdutoGateway>();
        _produtoUseCase = ProdutoUseCase.Create(_produtoGateway, _categoriaGateway);
    }

    [Fact]
    public async Task ListarProdutos_DeveRetornarProdutosAtivos_QuandoIncluirInativosForFalse()
    {
        // Arrange
        var produtosAtivos = new List<Produto>
        {
            new() { Id = Guid.NewGuid(), Nome = "Produto 1", Ativo = true },
            new() { Id = Guid.NewGuid(), Nome = "Produto 2", Ativo = true }
        };
        _produtoGateway.ListarProdutosAtivos().Returns(produtosAtivos);

        // Act
        var resultado = await _produtoUseCase.ListarProdutos(false);

        // Assert
        resultado.Should().HaveCount(2);
        resultado.Should().BeEquivalentTo(produtosAtivos);
        await _produtoGateway.Received(1).ListarProdutosAtivos();
    }

    [Fact]
    public async Task ListarProdutos_DeveRetornarTodosProdutos_QuandoIncluirInativosForTrue()
    {
        // Arrange
        var todosProdutos = new List<Produto>
        {
            new() { Id = Guid.NewGuid(), Nome = "Produto 1", Ativo = true },
            new() { Id = Guid.NewGuid(), Nome = "Produto 2", Ativo = false }
        };
        _produtoGateway.ListarTodosProdutos().Returns(todosProdutos);

        // Act
        var resultado = await _produtoUseCase.ListarProdutos(true);

        // Assert
        resultado.Should().HaveCount(2);
        resultado.Should().BeEquivalentTo(todosProdutos);
        await _produtoGateway.Received(1).ListarTodosProdutos();
    }

    [Fact]
    public async Task ListarProdutos_DeveRetornarProdutosPorCategoria_QuandoCategoriaIdFornecido()
    {
        // Arrange
        var categoriaId = Guid.NewGuid();
        var categoria = new CategoriaProduto { Id = categoriaId, Nome = "Categoria Teste" };
        var produtosDaCategoria = new List<Produto>
        {
            new() { Id = Guid.NewGuid(), Nome = "Produto 1", CategoriaId = categoriaId }
        };

        _categoriaGateway.ObterCategoriaPorId(categoriaId).Returns(categoria);
        _produtoGateway.ListarProdutosAtivosPorCategoria(categoriaId).Returns(produtosDaCategoria);

        // Act
        var resultado = await _produtoUseCase.ListarProdutos(false, categoriaId);

        // Assert
        resultado.Should().HaveCount(1);
        resultado.Should().BeEquivalentTo(produtosDaCategoria);
        await _categoriaGateway.Received(1).ObterCategoriaPorId(categoriaId);
        await _produtoGateway.Received(1).ListarProdutosAtivosPorCategoria(categoriaId);
    }

    [Fact]
    public async Task ListarProdutos_DeveLancarExcecao_QuandoCategoriaIdNaoExistir()
    {
        // Arrange
        var categoriaIdInexistente = Guid.NewGuid();
        _categoriaGateway.ObterCategoriaPorId(categoriaIdInexistente).Returns((CategoriaProduto?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _produtoUseCase.ListarProdutos(false, categoriaIdInexistente));
        
        exception.Message.Should().Be("Categoria não encontrada");
    }

    [Fact]
    public async Task ObterProdutoPorId_DeveRetornarProduto_QuandoProdutoExistir()
    {
        // Arrange
        var produtoId = Guid.NewGuid();
        var produto = new Produto { Id = produtoId, Nome = "Produto Teste" };
        _produtoGateway.ObterProdutoPorId(produtoId).Returns(produto);

        // Act
        var resultado = await _produtoUseCase.ObterProdutoPorId(produtoId);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Id.Should().Be(produtoId);
        resultado.Nome.Should().Be("Produto Teste");
    }

    [Fact]
    public async Task ObterProdutoPorId_DeveRetornarNull_QuandoProdutoNaoExistir()
    {
        // Arrange
        var produtoIdInexistente = Guid.NewGuid();
        _produtoGateway.ObterProdutoPorId(produtoIdInexistente).Returns((Produto?)null);

        // Act
        var resultado = await _produtoUseCase.ObterProdutoPorId(produtoIdInexistente);

        // Assert
        resultado.Should().BeNull();
    }

    [Fact]
    public async Task CriarProduto_DeveCriarProduto_ComDadosValidos()
    {
        // Arrange
        var categoriaId = Guid.NewGuid();
        var categoria = new CategoriaProduto { Id = categoriaId, Nome = "Categoria Teste" };
        var dto = new CriarProdutoDto
        {
            Nome = "Produto Novo",
            SKU = "PROD001",
            Descricao = "Descrição",
            Preco = 29.99m,
            CategoriaId = categoriaId,
            Imagem = "produto.jpg"
        };

        _produtoGateway.ProdutoExiste(dto.SKU).Returns(false);
        _categoriaGateway.ObterCategoriaPorId(categoriaId).Returns(categoria);

        // Act
        var resultado = await _produtoUseCase.CriarProduto(dto);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Nome.Should().Be(dto.Nome);
        resultado.SKU.Should().Be(dto.SKU);
        resultado.Preco.Should().Be(dto.Preco);
        resultado.Ativo.Should().BeTrue();
        await _produtoGateway.Received(1).CriarProduto(Arg.Any<Produto>());
    }

    [Fact]
    public async Task CriarProduto_DeveLancarExcecao_QuandoPrecoForZeroOuNegativo()
    {
        // Arrange
        var dto = new CriarProdutoDto
        {
            Nome = "Produto",
            SKU = "PROD001",
            Preco = 0m,
            CategoriaId = Guid.NewGuid()
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _produtoUseCase.CriarProduto(dto));
        
        exception.Message.Should().Be("O preço do produto deve ser maior que zero");
    }

    [Fact]
    public async Task CriarProduto_DeveLancarExcecao_QuandoSKUJaExistir()
    {
        // Arrange
        var dto = new CriarProdutoDto
        {
            Nome = "Produto",
            SKU = "PROD001",
            Preco = 10m,
            CategoriaId = Guid.NewGuid()
        };

        _produtoGateway.ProdutoExiste(dto.SKU).Returns(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _produtoUseCase.CriarProduto(dto));
        
        exception.Message.Should().Be("Produto com mesmo SKU já existe");
    }

    [Fact]
    public async Task CriarProduto_DeveLancarExcecao_QuandoCategoriaIdNaoExistir()
    {
        // Arrange
        var categoriaIdInexistente = Guid.NewGuid();
        var dto = new CriarProdutoDto
        {
            Nome = "Produto",
            SKU = "PROD001",
            Preco = 10m,
            CategoriaId = categoriaIdInexistente
        };

        _produtoGateway.ProdutoExiste(dto.SKU).Returns(false);
        _categoriaGateway.ObterCategoriaPorId(categoriaIdInexistente).Returns((CategoriaProduto?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
            _produtoUseCase.CriarProduto(dto));
        
        exception.Message.Should().Be("Categoria não encontrada");
    }

    [Fact]
    public async Task DesativarProduto_DeveDesativarProduto_QuandoProdutoExistir()
    {
        // Arrange
        var produtoId = Guid.NewGuid();
        var produto = new Produto { Id = produtoId, Nome = "Produto", Ativo = true };
        _produtoGateway.ObterProdutoPorId(produtoId).Returns(produto);

        // Act
        await _produtoUseCase.DesativarProduto(produtoId);

        // Assert
        produto.Ativo.Should().BeFalse();
        await _produtoGateway.Received(1).AtualizarProduto(produto);
    }

    [Fact]
    public async Task DesativarProduto_DeveLancarExcecao_QuandoProdutoNaoExistir()
    {
        // Arrange
        var produtoIdInexistente = Guid.NewGuid();
        _produtoGateway.ObterProdutoPorId(produtoIdInexistente).Returns((Produto?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _produtoUseCase.DesativarProduto(produtoIdInexistente));
        
        exception.Message.Should().Be("Produto não encontrado");
    }

    [Fact]
    public async Task ReativarProduto_DeveReativarProduto_QuandoProdutoExistir()
    {
        // Arrange
        var produtoId = Guid.NewGuid();
        var produto = new Produto { Id = produtoId, Nome = "Produto", Ativo = false };
        _produtoGateway.ObterProdutoPorId(produtoId).Returns(produto);

        // Act
        await _produtoUseCase.ReativarProduto(produtoId);

        // Assert
        produto.Ativo.Should().BeTrue();
        await _produtoGateway.Received(1).AtualizarProduto(produto);
    }

    [Fact]
    public async Task ReativarProduto_DeveLancarExcecao_QuandoProdutoNaoExistir()
    {
        // Arrange
        var produtoIdInexistente = Guid.NewGuid();
        _produtoGateway.ObterProdutoPorId(produtoIdInexistente).Returns((Produto?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _produtoUseCase.ReativarProduto(produtoIdInexistente));
        
        exception.Message.Should().Be("Produto não encontrado");
    }
}