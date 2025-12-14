using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra.Data;
using Soat.Eleven.FastFood.GestaoProdutos.Core.DTOs.Categorias;
using Soat.Eleven.FastFood.GestaoProdutos.Tests.Helpers;

namespace Soat.Eleven.FastFood.GestaoProdutos.Tests.IntegrationTests.Endpoints;

public class CategoriaProdutoEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public CategoriaProdutoEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    private async Task LimparBancoDados()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.CategoriasProdutos.RemoveRange(context.CategoriasProdutos);
        await context.SaveChangesAsync();
    }

    [Fact]
    public async Task GetCategorias_DeveRetornarListaCategorias_ComSucesso()
    {
        // Arrange
        await LimparBancoDados();
        
        var novaCategoria = new CriarCategoriaDto
        {
            Nome = "Bebidas Test",
            Descricao = "Categoria para bebidas de teste"
        };

        // Criar uma categoria primeiro
        await _client.PostAsJsonAsync("/api/Categoria", novaCategoria);

        // Act
        var response = await _client.GetAsync("/api/Categoria");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        var categorias = JsonSerializer.Deserialize<List<CategoriaProdutoDto>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        categorias.Should().NotBeNull();
        categorias.Should().HaveCountGreaterThanOrEqualTo(1);
        categorias.Should().Contain(c => c.Nome == "Bebidas Test");
    }

    [Fact]
    public async Task PostCategoria_DeveCriarCategoria_ComSucesso()
    {
        // Arrange
        await LimparBancoDados();
        
        var novaCategoria = new CriarCategoriaDto
        {
            Nome = "Lanches Test",
            Descricao = "Categoria para lanches de teste"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/Categoria", novaCategoria);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var content = await response.Content.ReadAsStringAsync();
        var categoriaCreated = JsonSerializer.Deserialize<CategoriaProdutoDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        categoriaCreated.Should().NotBeNull();
        categoriaCreated!.Nome.Should().Be(novaCategoria.Nome);
        categoriaCreated.Descricao.Should().Be(novaCategoria.Descricao);
        categoriaCreated.Ativo.Should().BeTrue();
        categoriaCreated.Id.Should().NotBe(Guid.Empty);
    }

    [Fact]
    public async Task GetCategoria_DeveRetornarCategoria_QuandoExiste()
    {
        // Arrange
        await LimparBancoDados();
        
        var novaCategoria = new CriarCategoriaDto
        {
            Nome = "Sobremesas Test",
            Descricao = "Categoria para sobremesas de teste"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Categoria", novaCategoria);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var categoriaCreated = JsonSerializer.Deserialize<CategoriaProdutoDto>(createContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Act
        var response = await _client.GetAsync($"/api/Categoria/{categoriaCreated!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        var categoria = JsonSerializer.Deserialize<CategoriaProdutoDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        categoria.Should().NotBeNull();
        categoria!.Id.Should().Be(categoriaCreated.Id);
        categoria.Nome.Should().Be(novaCategoria.Nome);
        categoria.Descricao.Should().Be(novaCategoria.Descricao);
    }

    [Fact]
    public async Task PutCategoria_DeveAtualizarCategoria_ComSucesso()
    {
        // Arrange
        await LimparBancoDados();
        
        var novaCategoria = new CriarCategoriaDto
        {
            Nome = "Categoria Original",
            Descricao = "Descrição original"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Categoria", novaCategoria);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var categoriaCreated = JsonSerializer.Deserialize<CategoriaProdutoDto>(createContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var atualizarCategoria = new AtualizarCategoriaDto
        {
            Nome = "Categoria Atualizada",
            Descricao = "Descrição atualizada"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/Categoria/{categoriaCreated!.Id}", atualizarCategoria);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        var categoriaAtualizada = JsonSerializer.Deserialize<CategoriaProdutoDto>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        categoriaAtualizada.Should().NotBeNull();
        categoriaAtualizada!.Id.Should().Be(categoriaCreated.Id);
        categoriaAtualizada.Nome.Should().Be(atualizarCategoria.Nome);
        categoriaAtualizada.Descricao.Should().Be(atualizarCategoria.Descricao);
    }

    [Fact]
    public async Task DeleteCategoria_DeveDesativarCategoria_ComSucesso()
    {
        // Arrange
        await LimparBancoDados();
        
        var novaCategoria = new CriarCategoriaDto
        {
            Nome = "Categoria para Desativar",
            Descricao = "Será desativada"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Categoria", novaCategoria);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var categoriaCreated = JsonSerializer.Deserialize<CategoriaProdutoDto>(createContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Act
        var response = await _client.DeleteAsync($"/api/Categoria/{categoriaCreated!.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verificar se foi desativada
        var getResponse = await _client.GetAsync($"/api/Categoria/{categoriaCreated.Id}");
        var getContent = await getResponse.Content.ReadAsStringAsync();
        var categoriaDesativada = JsonSerializer.Deserialize<CategoriaProdutoDto>(getContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        categoriaDesativada!.Ativo.Should().BeFalse();
    }

    [Fact]
    public async Task ReativarCategoria_DeveReativarCategoria_ComSucesso()
    {
        // Arrange
        await LimparBancoDados();
        
        var novaCategoria = new CriarCategoriaDto
        {
            Nome = "Categoria para Reativar",
            Descricao = "Será reativada"
        };

        var createResponse = await _client.PostAsJsonAsync("/api/Categoria", novaCategoria);
        var createContent = await createResponse.Content.ReadAsStringAsync();
        var categoriaCreated = JsonSerializer.Deserialize<CategoriaProdutoDto>(createContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Primeiro desativar
        await _client.DeleteAsync($"/api/Categoria/{categoriaCreated!.Id}");

        // Act - Reativar
        var response = await _client.PostAsync($"/api/Categoria/{categoriaCreated.Id}/reativar", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        // Verificar se foi reativada
        var getResponse = await _client.GetAsync($"/api/Categoria/{categoriaCreated.Id}");
        var getContent = await getResponse.Content.ReadAsStringAsync();
        var categoriaReativada = JsonSerializer.Deserialize<CategoriaProdutoDto>(getContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        categoriaReativada!.Ativo.Should().BeTrue();
    }

    [Fact]
    public async Task PutCategoria_DeveRetornarNotFound_QuandoCategoriaNaoExiste()
    {
        // Arrange
        var idInexistente = Guid.NewGuid();
        var atualizarCategoria = new AtualizarCategoriaDto
        {
            Nome = "Categoria Inexistente",
            Descricao = "Não existe"
        };

        // Act
        var response = await _client.PutAsJsonAsync($"/api/Categoria/{idInexistente}", atualizarCategoria);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteCategoria_DeveRetornarNotFound_QuandoCategoriaNaoExiste()
    {
        // Arrange
        var idInexistente = Guid.NewGuid();

        // Act
        var response = await _client.DeleteAsync($"/api/Categoria/{idInexistente}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}