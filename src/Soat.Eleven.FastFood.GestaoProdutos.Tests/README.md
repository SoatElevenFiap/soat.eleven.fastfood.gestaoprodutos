# Testes para Categoria de Produtos

Este projeto contém os testes automatizados para o módulo de **Categorias de Produtos** do sistema de gestão de fast food.

## Estrutura dos Testes

### Testes Unitários (`UnitTests/`)

- **Entities/CategoriaProdutoTests.cs**: Testa a entidade `CategoriaProduto`, incluindo validações e comportamentos.
- **UsesCases/CategoriaProdutoUseCaseTests.cs**: Testa as regras de negócio do `CategoriaProdutoUseCase`.
- **DTOs/CategoriaProdutoDtoTests.cs**: Testa os objetos de transferência de dados.

### Testes de Integração (`IntegrationTests/`)

- **DataSources/CategoriaProdutoDataSourceTests.cs**: Testa a integração com o banco de dados através do `CategoriaProdutoDataSource`.
- **Endpoints/CategoriaProdutoEndpointsTests.cs**: Testa os endpoints REST da API de categorias.

### Helpers (`Helpers/`)

- **CategoriaProdutoTestHelper.cs**: Métodos auxiliares para criar objetos de teste.
- **TestDbContextHelper.cs**: Configuração do contexto de banco em memória para testes de integração.
- **CustomWebApplicationFactory.cs**: Factory customizada para testes de integração da API.

## Tecnologias Utilizadas

- **xUnit**: Framework de testes
- **FluentAssertions**: Assertions mais expressivas
- **NSubstitute**: Mock framework
- **Entity Framework InMemory**: Banco em memória para testes
- **ASP.NET Core Testing**: Testes de integração da API

## Como Executar os Testes

### Via Visual Studio
1. Abra a solução no Visual Studio
2. Vá em `Test` > `Run All Tests` ou pressione `Ctrl + R, A`

### Via Terminal
```bash
# Navegar para a pasta da solução
cd src

# Executar todos os testes
dotnet test

# Executar apenas testes unitários
dotnet test --filter "UnitTests"

# Executar apenas testes de integração
dotnet test --filter "IntegrationTests"

# Executar com relatório de cobertura
dotnet test --collect:"XPlat Code Coverage"
```

## Cenários de Teste Cobertos

### Entidade CategoriaProduto
- ✅ Criação com propriedades válidas
- ✅ Validação de nome obrigatório
- ✅ Inicialização de valores padrão
- ✅ Gerenciamento do estado ativo/inativo

### Casos de Uso (CategoriaProdutoUseCase)
- ✅ Listagem de categorias ativas
- ✅ Listagem de todas as categorias
- ✅ Obtenção por ID
- ✅ Criação de nova categoria
- ✅ Validação de nome duplicado
- ✅ Atualização de categoria existente
- ✅ Desativação e reativação de categorias
- ✅ Tratamento de erros (categoria não encontrada)

### DataSource (Integração com BD)
- ✅ Inserção de categoria
- ✅ Busca por ID
- ✅ Listagem de todas as categorias
- ✅ Filtros customizados
- ✅ Atualização de categoria
- ✅ Remoção de categoria
- ✅ Tratamento de erros

### Endpoints REST (API)
- ✅ GET `/api/Categoria` - Listar categorias
- ✅ GET `/api/Categoria/{id}` - Obter categoria por ID
- ✅ POST `/api/Categoria` - Criar categoria
- ✅ PUT `/api/Categoria/{id}` - Atualizar categoria
- ✅ DELETE `/api/Categoria/{id}` - Desativar categoria
- ✅ POST `/api/Categoria/{id}/reativar` - Reativar categoria
- ✅ Códigos de status HTTP apropriados
- ✅ Validação de entrada
- ✅ Tratamento de cenários de erro

## Padrões de Nomenclatura

Os testes seguem o padrão **AAA (Arrange, Act, Assert)**:

```csharp
[Fact]
public void MetodoTestado_DeveComportamentoEsperado_QuandoCondicao()
{
    // Arrange - Preparação dos dados
    
    // Act - Execução do método testado
    
    // Assert - Verificação do resultado
}
```

## Executando Testes Específicos

Para executar categorias específicas de testes:

```bash
# Executar apenas testes de uma classe específica
dotnet test --filter "CategoriaProdutoTests"

# Executar um teste específico
dotnet test --filter "CategoriaProduto_DeveSerCriadaComSucesso_QuandoPropriedadesValidas"
```