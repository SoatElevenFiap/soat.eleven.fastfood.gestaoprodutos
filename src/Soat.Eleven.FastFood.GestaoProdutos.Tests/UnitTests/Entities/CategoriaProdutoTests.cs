using Soat.Eleven.FastFood.GestaoProdutos.Core.Entities;
using Soat.Eleven.FastFood.GestaoProdutos.Tests.Helpers;

namespace Soat.Eleven.FastFood.GestaoProdutos.Tests.UnitTests.Entities;

public class CategoriaProdutoTests
{
    [Fact]
    public void CategoriaProduto_DeveSerCriadaComSucesso_QuandoPropriedadesValidas()
    {
        // Arrange
        var id = Guid.NewGuid();
        var nome = "Bebidas";
        var descricao = "Categoria para bebidas";
        var ativo = true;
        var criadoEm = DateTime.UtcNow;

        // Act
        var categoria = new CategoriaProduto
        {
            Id = id,
            Nome = nome,
            Descricao = descricao,
            Ativo = ativo,
            CriadoEm = criadoEm,
            ModificadoEm = criadoEm
        };

        // Assert
        categoria.Id.Should().Be(id);
        categoria.Nome.Should().Be(nome);
        categoria.Descricao.Should().Be(descricao);
        categoria.Ativo.Should().Be(ativo);
        categoria.CriadoEm.Should().Be(criadoEm);
        categoria.ModificadoEm.Should().Be(criadoEm);
        categoria.Produtos.Should().NotBeNull();
        categoria.Produtos.Should().BeEmpty();
    }

    [Fact]
    public void CategoriaProduto_DeveInicializarComoAtiva_PorPadrao()
    {
        // Arrange & Act
        var categoria = new CategoriaProduto();

        // Assert
        categoria.Ativo.Should().BeTrue();
        categoria.Produtos.Should().NotBeNull();
    }

    [Theory]
    [InlineData("")]
    public void CategoriaProduto_DeveLancarExcecao_QuandoNomeVazio(string nomeInvalido)
    {
        // Arrange
        var categoria = new CategoriaProduto();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => categoria.Nome = nomeInvalido);
        exception.Message.Should().Contain("Nome");
    }

    [Fact]
    public void CategoriaProduto_DeveLancarExcecao_QuandoNomeENull()
    {
        // Arrange
        var categoria = new CategoriaProduto();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => categoria.Nome = null!);
        exception.Message.Should().Contain("Nome");
    }

    [Fact]
    public void CategoriaProduto_DevePermitirNomeValido()
    {
        // Arrange
        var categoria = new CategoriaProduto();
        var nomeValido = "Bebidas";

        // Act
        categoria.Nome = nomeValido;

        // Assert
        categoria.Nome.Should().Be(nomeValido);
    }

    [Fact]
    public void CategoriaProduto_DevePermitirDescricaoNula()
    {
        // Arrange
        var categoria = new CategoriaProduto();

        // Act
        categoria.Descricao = null;

        // Assert
        categoria.Descricao.Should().BeNull();
    }

    [Fact]
    public void CategoriaProduto_DevePermitirDesativarCategoria()
    {
        // Arrange
        var categoria = CategoriaProdutoTestHelper.CreateValidCategoriaProduto();

        // Act
        categoria.Ativo = false;

        // Assert
        categoria.Ativo.Should().BeFalse();
    }

    [Fact]
    public void CategoriaProduto_DeveTerColecaoProdutosInicializada()
    {
        // Arrange & Act
        var categoria = new CategoriaProduto();

        // Assert
        categoria.Produtos.Should().NotBeNull();
        categoria.Produtos.Should().BeOfType<List<Produto>>();
    }
}