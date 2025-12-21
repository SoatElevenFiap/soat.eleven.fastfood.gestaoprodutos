using Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Categorias;

namespace Soat.Eleven.FastFood.GestaoProdutos.Tests.UnitTests.DTOs;

public class CategoriaProdutoDtoTests
{
    [Fact]
    public void CategoriaProdutoDto_DeveSerCriadaComSucesso_ComPropriedadesValidas()
    {
        // Arrange
        var id = Guid.NewGuid();
        var nome = "Bebidas";
        var descricao = "Categoria para bebidas";
        var ativo = true;

        // Act
        var dto = new CategoriaProdutoDto
        {
            Id = id,
            Nome = nome,
            Descricao = descricao,
            Ativo = ativo
        };

        // Assert
        dto.Id.Should().Be(id);
        dto.Nome.Should().Be(nome);
        dto.Descricao.Should().Be(descricao);
        dto.Ativo.Should().Be(ativo);
    }

    [Fact]
    public void CategoriaProdutoDto_DevePermitirDescricaoNula()
    {
        // Arrange & Act
        var dto = new CategoriaProdutoDto
        {
            Id = Guid.NewGuid(),
            Nome = "Categoria Teste",
            Descricao = null,
            Ativo = true
        };

        // Assert
        dto.Descricao.Should().BeNull();
    }

    [Fact]
    public void CategoriaProdutoDto_DevePermitirIdVazio()
    {
        // Arrange & Act
        var dto = new CategoriaProdutoDto
        {
            Id = Guid.Empty,
            Nome = "Categoria Teste",
            Descricao = "Descrição teste",
            Ativo = true
        };

        // Assert
        dto.Id.Should().Be(Guid.Empty);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void CategoriaProdutoDto_DevePermitirQualquerValorParaAtivo(bool ativo)
    {
        // Arrange & Act
        var dto = new CategoriaProdutoDto
        {
            Id = Guid.NewGuid(),
            Nome = "Categoria Teste",
            Descricao = "Descrição teste",
            Ativo = ativo
        };

        // Assert
        dto.Ativo.Should().Be(ativo);
    }

    [Fact]
    public void CategoriaProdutoDto_DeveInicializarComValoresPadrao()
    {
        // Arrange & Act
        var dto = new CategoriaProdutoDto();

        // Assert
        dto.Id.Should().Be(Guid.Empty);
        dto.Nome.Should().BeNull();
        dto.Descricao.Should().BeNull();
        dto.Ativo.Should().BeFalse(); // bool default value
    }
}