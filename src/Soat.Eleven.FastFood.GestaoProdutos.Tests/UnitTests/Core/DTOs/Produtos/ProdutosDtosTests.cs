using FluentAssertions;
using Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Produtos;
using System.Text.Json;
using Xunit;

namespace Soat.Eleven.FastFood.GestaoProdutos.Tests.UnitTests.Core.DTOs.Produtos;

public class CriarProdutoDtoTests
{
    [Fact]
    public void CriarProdutoDto_DevePermitir_CriarInstanciaValida()
    {
        // Arrange
        var categoriaId = Guid.NewGuid();
        
        // Act
        var dto = new CriarProdutoDto
        {
            Nome = "Produto Teste",
            SKU = "PROD001",
            Descricao = "Descrição do produto",
            Preco = 29.99m,
            Imagem = "produto.jpg",
            CategoriaId = categoriaId
        };

        // Assert
        dto.Nome.Should().Be("Produto Teste");
        dto.SKU.Should().Be("PROD001");
        dto.Descricao.Should().Be("Descrição do produto");
        dto.Preco.Should().Be(29.99m);
        dto.Imagem.Should().Be("produto.jpg");
        dto.CategoriaId.Should().Be(categoriaId);
    }

    [Fact]
    public void CriarProdutoDto_DevePermitir_ImagemNula()
    {
        // Arrange & Act
        var dto = new CriarProdutoDto
        {
            Nome = "Produto",
            SKU = "PROD001",
            Descricao = "Descrição",
            Preco = 10.00m,
            CategoriaId = Guid.NewGuid(),
            Imagem = null
        };

        // Assert
        dto.Imagem.Should().BeNull();
    }

    [Fact]
    public void CriarProdutoDto_DevePermitir_DescricaoVazia()
    {
        // Arrange & Act
        var dto = new CriarProdutoDto
        {
            Nome = "Produto",
            SKU = "PROD001",
            Descricao = string.Empty,
            Preco = 10.00m,
            CategoriaId = Guid.NewGuid()
        };

        // Assert
        dto.Descricao.Should().Be(string.Empty);
    }
}

public class AtualizarProdutoDtoTests
{
    [Fact]
    public void AtualizarProdutoDto_DevePermitir_CriarInstanciaValida()
    {
        // Arrange
        var id = Guid.NewGuid();
        var categoriaId = Guid.NewGuid();
        
        // Act
        var dto = new AtualizarProdutoDto
        {
            Id = id,
            Nome = "Produto Atualizado",
            SKU = "PROD002",
            Descricao = "Nova descrição",
            Preco = 39.99m,
            Imagem = "novo_produto.jpg",
            CategoriaId = categoriaId
        };

        // Assert
        dto.Id.Should().Be(id);
        dto.Nome.Should().Be("Produto Atualizado");
        dto.SKU.Should().Be("PROD002");
        dto.Descricao.Should().Be("Nova descrição");
        dto.Preco.Should().Be(39.99m);
        dto.Imagem.Should().Be("novo_produto.jpg");
        dto.CategoriaId.Should().Be(categoriaId);
    }

    [Fact]
    public void AtualizarProdutoDto_ImagemFoiEnviada_DeveRetornarTrue_QuandoImagemNosCamposExtras()
    {
        // Arrange
        var dto = new AtualizarProdutoDto();
        dto.CamposExtras = new Dictionary<string, JsonElement>
        {
            { "Imagem", JsonDocument.Parse("\"nova_imagem.jpg\"").RootElement }
        };

        // Act
        var resultado = dto.ImagemFoiEnviada();

        // Assert
        resultado.Should().BeTrue();
    }

    [Fact]
    public void AtualizarProdutoDto_ImagemFoiEnviada_DeveRetornarFalse_QuandoImagemNaoNosCamposExtras()
    {
        // Arrange
        var dto = new AtualizarProdutoDto();
        dto.CamposExtras = new Dictionary<string, JsonElement>
        {
            { "OutroCampo", JsonDocument.Parse("\"valor\"").RootElement }
        };

        // Act
        var resultado = dto.ImagemFoiEnviada();

        // Assert
        resultado.Should().BeFalse();
    }

    [Fact]
    public void AtualizarProdutoDto_ImagemFoiEnviada_DeveRetornarFalse_QuandoCamposExtrasNulo()
    {
        // Arrange
        var dto = new AtualizarProdutoDto
        {
            CamposExtras = null
        };

        // Act
        var resultado = dto.ImagemFoiEnviada();

        // Assert
        resultado.Should().BeFalse();
    }

    [Fact]
    public void AtualizarProdutoDto_ImagemFoiEnviada_DeveRetornarFalse_QuandoCamposExtrasVazio()
    {
        // Arrange
        var dto = new AtualizarProdutoDto
        {
            CamposExtras = new Dictionary<string, JsonElement>()
        };

        // Act
        var resultado = dto.ImagemFoiEnviada();

        // Assert
        resultado.Should().BeFalse();
    }
}

public class ProdutoDtoTests
{
    [Fact]
    public void ProdutoDto_DevePermitir_CriarInstanciaValida()
    {
        // Arrange
        var id = Guid.NewGuid();
        var categoriaId = Guid.NewGuid();
        
        // Act
        var dto = new ProdutoDto
        {
            Id = id,
            Nome = "Produto DTO",
            SKU = "PROD003",
            Descricao = "Descrição DTO",
            Preco = 49.99m,
            CategoriaId = categoriaId,
            Ativo = true,
            Imagem = "produto_dto.jpg"
        };

        // Assert
        dto.Id.Should().Be(id);
        dto.Nome.Should().Be("Produto DTO");
        dto.SKU.Should().Be("PROD003");
        dto.Descricao.Should().Be("Descrição DTO");
        dto.Preco.Should().Be(49.99m);
        dto.CategoriaId.Should().Be(categoriaId);
        dto.Ativo.Should().BeTrue();
        dto.Imagem.Should().Be("produto_dto.jpg");
    }

    [Fact]
    public void ProdutoDto_DevePermitir_DescricaoNula()
    {
        // Arrange & Act
        var dto = new ProdutoDto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto",
            SKU = "PROD001",
            Descricao = null,
            Preco = 10.00m,
            CategoriaId = Guid.NewGuid(),
            Ativo = true
        };

        // Assert
        dto.Descricao.Should().BeNull();
    }

    [Fact]
    public void ProdutoDto_DevePermitir_ImagemNula()
    {
        // Arrange & Act
        var dto = new ProdutoDto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto",
            SKU = "PROD001",
            Preco = 10.00m,
            CategoriaId = Guid.NewGuid(),
            Ativo = true,
            Imagem = null
        };

        // Assert
        dto.Imagem.Should().BeNull();
    }
}