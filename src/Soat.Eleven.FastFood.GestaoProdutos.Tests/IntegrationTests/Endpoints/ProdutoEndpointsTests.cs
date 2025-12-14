using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.Data;
using Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Categorias;
using Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Produtos;
using Soat.Eleven.FastFood.GestaoProdutos.Tests.Helpers;

namespace Soat.Eleven.FastFood.GestaoProdutos.Tests.IntegrationTests.Endpoints;

public class ProdutoEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ProdutoEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    private async Task LimparBancoDados()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        context.Produtos.RemoveRange(context.Produtos);
        context.CategoriasProdutos.RemoveRange(context.CategoriasProdutos);
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetProdutos_DeveRetornarListaProdutos_ComSucesso()
    {
        // Arrange
        await LimparBancoDados();
        var categoria = await CriarCategoriaAsync();
        await CriarProdutoAsync(categoria.Id, "Produto 1", "PROD001");
        await CriarProdutoAsync(categoria.Id, "Produto 2", "PROD002");

        // Act
        var response = await _client.GetAsync("/api/Produto");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var produtos = await response.Content.ReadFromJsonAsync<List<ProdutoDto>>();
        produtos.Should().HaveCount(2);
        produtos.Should().Contain(p => p.Nome == "Produto 1");
        produtos.Should().Contain(p => p.Nome == "Produto 2");
    }

    [Fact]
    public async Task PostProduto_DeveCriarProduto_ComSucesso()
    {
        // Arrange
        await LimparBancoDados();
        var categoria = await CriarCategoriaAsync();
        var novoProduto = new CriarProdutoDto
        {
            Nome = "Produto Novo",
            SKU = "PROD003",
            Descricao = "Descrição do produto novo",
            Preco = 39.99m,
            CategoriaId = categoria.Id,
            Imagem = "produto_novo.jpg"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Produto", novoProduto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var produtoCriado = await response.Content.ReadFromJsonAsync<ProdutoDto>();
        produtoCriado.Should().NotBeNull();
        produtoCriado!.Nome.Should().Be(novoProduto.Nome);
        produtoCriado.SKU.Should().Be(novoProduto.SKU);
        produtoCriado.Preco.Should().Be(novoProduto.Preco);
        produtoCriado.Ativo.Should().BeTrue();
    }

    [Fact]
    public async Task GetProduto_DeveRetornarProduto_QuandoExiste()
    {
        // Arrange
        await LimparBancoDados();
        var categoria = await CriarCategoriaAsync();
        var produto = await CriarProdutoAsync(categoria.Id, "Produto Existente", "PROD004");

        // Act
        var response = await _client.GetAsync($"/api/Produto/{produto.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var produtoRetornado = await response.Content.ReadFromJsonAsync<ProdutoDto>();
        produtoRetornado.Should().NotBeNull();
        produtoRetornado!.Id.Should().Be(produto.Id);
        produtoRetornado.Nome.Should().Be("Produto Existente");
    }

    [Fact]
    public async Task PutProduto_DeveAtualizarProduto_ComSucesso()
    {
        // Arrange
        await LimparBancoDados();
        var categoria = await CriarCategoriaAsync();
        var produto = await CriarProdutoAsync(categoria.Id, "Produto Original", "PROD005");

        var produtoAtualizado = new AtualizarProdutoDto
        {
            Id = produto.Id,
            Nome = "Produto Atualizado",
            SKU = "PROD005",
            Descricao = "Nova descrição",
            Preco = 49.99m,
            CategoriaId = categoria.Id,
            Imagem = "nova_imagem.jpg"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/Produto/{produto.Id}", produtoAtualizado);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var produtoResponse = await response.Content.ReadFromJsonAsync<ProdutoDto>();
        produtoResponse.Should().NotBeNull();
        produtoResponse!.Nome.Should().Be("Produto Atualizado");
        produtoResponse.Preco.Should().Be(49.99m);
    }

    [Fact]
    public async Task PutProduto_DeveRetornarNotFound_QuandoProdutoNaoExiste()
    {
        // Arrange
        await LimparBancoDados();
        var categoria = await CriarCategoriaAsync();
        var produtoInexistente = new AtualizarProdutoDto
        {
            Id = Guid.NewGuid(),
            Nome = "Produto Inexistente",
            SKU = "PROD999",
            CategoriaId = categoria.Id
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/Produto/{produtoInexistente.Id}", produtoInexistente);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteProduto_DeveDesativarProduto_ComSucesso()
    {
        // Arrange
        await LimparBancoDados();
        var categoria = await CriarCategoriaAsync();
        var produto = await CriarProdutoAsync(categoria.Id, "Produto para Deletar", "PROD006");

        // Act
        var response = await _client.DeleteAsync($"/api/Produto/{produto.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verificar se o produto foi desativado
        var produtoVerificacao = await _client.GetAsync($"/api/Produto/{produto.Id}");
        var produtoDesativado = await produtoVerificacao.Content.ReadFromJsonAsync<ProdutoDto>();
        produtoDesativado!.Ativo.Should().BeFalse();
    }

    [Fact]
    public async Task DeleteProduto_DeveRetornarNotFound_QuandoProdutoNaoExiste()
    {
        // Arrange
        await LimparBancoDados();
        var idInexistente = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"/api/Produto/{idInexistente}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ReativarProduto_DeveReativarProduto_ComSucesso()
    {
        // Arrange
        await LimparBancoDados();
        var categoria = await CriarCategoriaAsync();
        var produto = await CriarProdutoAsync(categoria.Id, "Produto para Reativar", "PROD007");

        // Primeiro desativar o produto
        await _client.DeleteAsync($"/api/Produto/{produto.Id}");

        // Act - Reativar o produto
        var response = await _client.PostAsync($"/api/Produto/{produto.Id}/reativar", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verificar se o produto foi reativado
        var produtoVerificacao = await _client.GetAsync($"/api/Produto/{produto.Id}");
        var produtoReativado = await produtoVerificacao.Content.ReadFromJsonAsync<ProdutoDto>();
        produtoReativado!.Ativo.Should().BeTrue();
    }

    [Fact]
    public async Task ReativarProduto_DeveRetornarNotFound_QuandoProdutoNaoExiste()
    {
        // Arrange
        await LimparBancoDados();
        var idInexistente = Guid.NewGuid();

        // Act
        var response = await _client.PostAsync($"/api/Produto/{idInexistente}/reativar", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task GetProdutos_DeveFiltrarPorCategoria_ComSucesso()
    {
        // Arrange
        await LimparBancoDados();
        var categoria1 = await CriarCategoriaAsync("Categoria 1");
        var categoria2 = await CriarCategoriaAsync("Categoria 2");
        
        await CriarProdutoAsync(categoria1.Id, "Produto Cat 1", "PROD008");
        await CriarProdutoAsync(categoria2.Id, "Produto Cat 2", "PROD009");

        // Act
        var response = await _client.GetAsync($"/api/Produto?categoriaId={categoria1.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var produtos = await response.Content.ReadFromJsonAsync<List<ProdutoDto>>();
        produtos.Should().HaveCount(1);
        produtos![0].Nome.Should().Be("Produto Cat 1");
        produtos[0].CategoriaId.Should().Be(categoria1.Id);
    }

    [Fact]
    public async Task PostProduto_DeveRetornarBadRequest_QuandoPrecoInvalido()
    {
        // Arrange
        await LimparBancoDados();
        var categoria = await CriarCategoriaAsync();
        var produtoInvalido = new CriarProdutoDto
        {
            Nome = "Produto Inválido",
            SKU = "PROD010",
            Preco = 0m, // Preço inválido
            CategoriaId = categoria.Id
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Produto", produtoInvalido);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private async Task<CategoriaProdutoDto> CriarCategoriaAsync(string nome = "Categoria Teste")
    {
        var categoria = new CriarCategoriaDto
        {
            Nome = nome,
            Descricao = "Descrição da categoria"
        };

        var response = await _client.PostAsJsonAsync("/api/Categoria", categoria);
        return (await response.Content.ReadFromJsonAsync<CategoriaProdutoDto>())!;
    }

    private async Task<ProdutoDto> CriarProdutoAsync(Guid categoriaId, string nome, string sku)
    {
        var produto = new CriarProdutoDto
        {
            Nome = nome,
            SKU = sku,
            Descricao = "Descrição do produto",
            Preco = 29.99m,
            CategoriaId = categoriaId,
            Imagem = "produto.jpg"
        };

        var response = await _client.PostAsJsonAsync("/api/Produto", produto);
        return (await response.Content.ReadFromJsonAsync<ProdutoDto>())!;
    }
}