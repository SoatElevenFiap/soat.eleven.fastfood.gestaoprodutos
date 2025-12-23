using Soat.Eleven.FastFood.GestaoProdutos.Core.Entities;
using Soat.Eleven.FastFood.GestaoProdutos.Core.Interfaces.Gateways;
using Soat.Eleven.FastFood.GestaoProdutos.Core.UsesCases;
using Soat.Eleven.FastFood.GestaoProdutos.Tests.Helpers;

namespace Soat.Eleven.FastFood.GestaoProdutos.Tests.UnitTests.UsesCases;

public class CategoriaProdutoUseCaseTests
{
    private readonly ICategoriaProdutoGateway _mockGateway;
    private readonly CategoriaProdutoUseCase _useCase;

    public CategoriaProdutoUseCaseTests()
    {
        _mockGateway = Substitute.For<ICategoriaProdutoGateway>();
        _useCase = CategoriaProdutoUseCase.Create(_mockGateway);
    }

    [Fact]
    public async Task ListarCategorias_DeveRetornarCategoriasAtivas_QuandoIncluirInativosNaoInformado()
    {
        // Arrange
        var categoriasAtivas = CategoriaProdutoTestHelper.CreateCategoriaProdutoList(2)
            .Where(c => c.Ativo).ToList();
        
        _mockGateway.ListarCategoriasAtivas().Returns(categoriasAtivas);

        // Act
        var resultado = await _useCase.ListarCategorias();

        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(categoriasAtivas.Count);
        resultado.All(c => c.Ativo).Should().BeTrue();
        await _mockGateway.Received(1).ListarCategoriasAtivas();
        await _mockGateway.DidNotReceive().ListarTodasCategorias();
    }

    [Fact]
    public async Task ListarCategorias_DeveRetornarCategoriasAtivas_QuandoIncluirInativosEFalse()
    {
        // Arrange
        var categoriasAtivas = CategoriaProdutoTestHelper.CreateCategoriaProdutoList(2)
            .Where(c => c.Ativo).ToList();
        
        _mockGateway.ListarCategoriasAtivas().Returns(categoriasAtivas);

        // Act
        var resultado = await _useCase.ListarCategorias(false);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(categoriasAtivas.Count);
        resultado.All(c => c.Ativo).Should().BeTrue();
        await _mockGateway.Received(1).ListarCategoriasAtivas();
        await _mockGateway.DidNotReceive().ListarTodasCategorias();
    }

    [Fact]
    public async Task ListarCategorias_DeveRetornarTodasCategorias_QuandoIncluirInativosETrue()
    {
        // Arrange
        var todasCategorias = CategoriaProdutoTestHelper.CreateCategoriaProdutoList(3);
        
        _mockGateway.ListarTodasCategorias().Returns(todasCategorias);

        // Act
        var resultado = await _useCase.ListarCategorias(true);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Should().HaveCount(todasCategorias.Count);
        await _mockGateway.Received(1).ListarTodasCategorias();
        await _mockGateway.DidNotReceive().ListarCategoriasAtivas();
    }

    [Fact]
    public async Task ObterCategoriaPorId_DeveRetornarCategoria_QuandoExiste()
    {
        // Arrange
        var categoria = CategoriaProdutoTestHelper.CreateValidCategoriaProduto();
        var id = categoria.Id;
        
        _mockGateway.ObterCategoriaPorId(id).Returns(categoria);

        // Act
        var resultado = await _useCase.ObterCategoriaPorId(id);

        // Assert
        resultado.Should().NotBeNull();
        resultado!.Id.Should().Be(id);
        resultado.Nome.Should().Be(categoria.Nome);
        await _mockGateway.Received(1).ObterCategoriaPorId(id);
    }

    [Fact]
    public async Task ObterCategoriaPorId_DeveRetornarNull_QuandoNaoExiste()
    {
        // Arrange
        var id = Guid.NewGuid();
        _mockGateway.ObterCategoriaPorId(id).Returns((CategoriaProduto?)null);

        // Act
        var resultado = await _useCase.ObterCategoriaPorId(id);

        // Assert
        resultado.Should().BeNull();
        await _mockGateway.Received(1).ObterCategoriaPorId(id);
    }

    [Fact]
    public async Task CriarCategoria_DeveCriarComSucesso_QuandoNaoExisteCategoriaMesmoNome()
    {
        // Arrange
        var nome = "Nova Categoria";
        var descricao = "Descrição da nova categoria";
        
        _mockGateway.CategoriaExiste(nome).Returns(false);

        // Act
        var resultado = await _useCase.CriarCategoria(nome, descricao);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Nome.Should().Be(nome);
        resultado.Descricao.Should().Be(descricao);
        resultado.Ativo.Should().BeTrue();
        resultado.Id.Should().NotBe(Guid.Empty);
        
        await _mockGateway.Received(1).CategoriaExiste(nome);
        await _mockGateway.Received(1).CriarCategoria(Arg.Is<CategoriaProduto>(c => 
            c.Nome == nome && c.Descricao == descricao && c.Ativo));
    }

    [Fact]
    public async Task CriarCategoria_DeveLancarExcecao_QuandoJaExisteCategoriaMesmoNome()
    {
        // Arrange
        var nome = "Categoria Existente";
        var descricao = "Descrição";
        
        _mockGateway.CategoriaExiste(nome).Returns(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => 
            _useCase.CriarCategoria(nome, descricao));
        
        exception.Message.Should().Contain("Categoria de mesmo nome já existe");
        await _mockGateway.Received(1).CategoriaExiste(nome);
        await _mockGateway.DidNotReceive().CriarCategoria(Arg.Any<CategoriaProduto>());
    }

    [Fact]
    public async Task AtualizarCategoria_DeveAtualizarComSucesso_QuandoCategoriaExiste()
    {
        // Arrange
        var categoria = CategoriaProdutoTestHelper.CreateValidCategoriaProduto();
        var novoNome = "Nome Atualizado";
        var novaDescricao = "Descrição Atualizada";
        
        _mockGateway.ObterCategoriaPorId(categoria.Id).Returns(categoria);

        // Act
        var resultado = await _useCase.AtualizarCategoria(categoria.Id, novoNome, novaDescricao);

        // Assert
        resultado.Should().NotBeNull();
        resultado.Id.Should().Be(categoria.Id);
        resultado.Nome.Should().Be(novoNome);
        resultado.Descricao.Should().Be(novaDescricao);
        
        await _mockGateway.Received(1).ObterCategoriaPorId(categoria.Id);
        await _mockGateway.Received(1).AtualizarCategoria(categoria);
    }

    [Fact]
    public async Task AtualizarCategoria_DeveLancarExcecao_QuandoCategoriaNaoExiste()
    {
        // Arrange
        var id = Guid.NewGuid();
        var nome = "Nome";
        var descricao = "Descrição";
        
        _mockGateway.ObterCategoriaPorId(id).Returns((CategoriaProduto?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _useCase.AtualizarCategoria(id, nome, descricao));
        
        exception.Message.Should().Contain("Categoria não encontrada");
        await _mockGateway.Received(1).ObterCategoriaPorId(id);
        await _mockGateway.DidNotReceive().AtualizarCategoria(Arg.Any<CategoriaProduto>());
    }

    [Fact]
    public async Task DesativarCategoria_DeveDesativarComSucesso_QuandoCategoriaExiste()
    {
        // Arrange
        var categoria = CategoriaProdutoTestHelper.CreateValidCategoriaProduto(ativo: true);
        
        _mockGateway.ObterCategoriaPorId(categoria.Id).Returns(categoria);

        // Act
        await _useCase.DesativarCategoria(categoria.Id);

        // Assert
        categoria.Ativo.Should().BeFalse();
        await _mockGateway.Received(1).ObterCategoriaPorId(categoria.Id);
        await _mockGateway.Received(1).AtualizarCategoria(categoria);
    }

    [Fact]
    public async Task DesativarCategoria_DeveLancarExcecao_QuandoCategoriaNaoExiste()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        _mockGateway.ObterCategoriaPorId(id).Returns((CategoriaProduto?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _useCase.DesativarCategoria(id));
        
        exception.Message.Should().Contain("Categoria não encontrada");
        await _mockGateway.Received(1).ObterCategoriaPorId(id);
        await _mockGateway.DidNotReceive().AtualizarCategoria(Arg.Any<CategoriaProduto>());
    }

    [Fact]
    public async Task ReativarCategoria_DeveReativarComSucesso_QuandoCategoriaExiste()
    {
        // Arrange
        var categoria = CategoriaProdutoTestHelper.CreateValidCategoriaProduto(ativo: false);
        
        _mockGateway.ObterCategoriaPorId(categoria.Id).Returns(categoria);

        // Act
        await _useCase.ReativarCategoria(categoria.Id);

        // Assert
        categoria.Ativo.Should().BeTrue();
        await _mockGateway.Received(1).ObterCategoriaPorId(categoria.Id);
        await _mockGateway.Received(1).AtualizarCategoria(categoria);
    }

    [Fact]
    public async Task ReativarCategoria_DeveLancarExcecao_QuandoCategoriaNaoExiste()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        _mockGateway.ObterCategoriaPorId(id).Returns((CategoriaProduto?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => 
            _useCase.ReativarCategoria(id));
        
        exception.Message.Should().Contain("Categoria não encontrada");
        await _mockGateway.Received(1).ObterCategoriaPorId(id);
        await _mockGateway.DidNotReceive().AtualizarCategoria(Arg.Any<CategoriaProduto>());
    }
}