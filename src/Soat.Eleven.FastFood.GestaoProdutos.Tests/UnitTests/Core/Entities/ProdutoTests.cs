using FluentAssertions;
using Soat.Eleven.FastFood.GestaoProdutos.Core.ConditionRules;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Entities;
using Xunit;

namespace Soat.Eleven.FastFood.GestaoProdutos.Tests.UnitTests.Core.Entities;

public class ProdutoTests
{
    [Fact]
    public void Produto_DevePermitir_CriarInstanciaValida()
    {
        // Arrange & Act
        var categoriaId = Guid.NewGuid();
        var produto = new Produto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto Teste",
            SKU = "PROD001",
            Descricao = "Descrição do produto",
            Preco = 29.99m,
            CategoriaId = categoriaId,
            Ativo = true,
            Imagem = "produto.jpg",
            CriadoEm = DateTime.UtcNow,
            ModificadoEm = DateTime.UtcNow
        };

        // Assert
        produto.Id.Should().NotBe(Guid.Empty);
        produto.Nome.Should().Be("Produto Teste");
        produto.SKU.Should().Be("PROD001");
        produto.Descricao.Should().Be("Descrição do produto");
        produto.Preco.Should().Be(29.99m);
        produto.CategoriaId.Should().Be(categoriaId);
        produto.Ativo.Should().BeTrue();
        produto.Imagem.Should().Be("produto.jpg");
    }

    [Theory]
    [InlineData("")]
    public void Produto_DeveLancarExcecao_QuandoNomeForInvalido(string nomeInvalido)
    {
        // Arrange & Act & Assert
        var action = () => new Produto { Nome = nomeInvalido };
        
        action.Should().Throw<ArgumentException>()
              .WithMessage("*Nome*");
    }

    [Fact]
    public void Produto_DeveLancarExcecao_QuandoNomeForNulo()
    {
        // Arrange & Act & Assert
        var action = () => new Produto { Nome = null! };
        
        action.Should().Throw<ArgumentException>()
              .WithMessage("*Nome*");
    }

    [Fact]
    public void Produto_DevePermitir_DefinirNomeValido()
    {
        // Arrange
        var produto = new Produto();
        var nomeValido = "Produto Válido";

        // Act
        produto.Nome = nomeValido;

        // Assert
        produto.Nome.Should().Be(nomeValido);
    }

    [Fact]
    public void Produto_DevePermitir_DefinirSKUValido()
    {
        // Arrange
        var produto = new Produto();
        var skuValido = "PROD123";

        // Act
        produto.SKU = skuValido;

        // Assert
        produto.SKU.Should().Be(skuValido);
    }

    [Fact]
    public void Produto_DevePermitir_DefinirSKUVazio()
    {
        // Arrange
        var produto = new Produto();

        // Act
        produto.SKU = "";

        // Assert
        produto.SKU.Should().Be("");
    }

    [Fact]
    public void Produto_DevePermitir_DefinirDescricaoNula()
    {
        // Arrange
        var produto = new Produto();

        // Act
        produto.Descricao = null;

        // Assert
        produto.Descricao.Should().BeNull();
    }

    [Fact]
    public void Produto_DevePermitir_DefinirImagemNula()
    {
        // Arrange
        var produto = new Produto();

        // Act
        produto.Imagem = null;

        // Assert
        produto.Imagem.Should().BeNull();
    }

    [Fact]
    public void Produto_DevePermitir_DefinirPrecoValido()
    {
        // Arrange
        var produto = new Produto();
        var preco = 99.99m;

        // Act
        produto.Preco = preco;

        // Assert
        produto.Preco.Should().Be(preco);
    }

    [Fact]
    public void Produto_DevePermitir_DefinirCategoriaIdValido()
    {
        // Arrange
        var produto = new Produto();
        var categoriaId = Guid.NewGuid();

        // Act
        produto.CategoriaId = categoriaId;

        // Assert
        produto.CategoriaId.Should().Be(categoriaId);
    }
}