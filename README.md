# FastFood - Microsserviço de Gestão de Produtos

## Sobre o Projeto

Este repositório contém o **microsserviço de Gestão de Produtos** da solução **FastFood**, uma aplicação desenvolvida para lanchonetes de fast food como parte do projeto de pós-graduação em **Arquitetura de Software** da **FIAP**.

O microsserviço é responsável pelo gerenciamento completo do catálogo de produtos, incluindo categorias de produtos, cadastro, atualização e consulta de produtos disponíveis para a lanchonete, integrando-se com outros microsserviços da solução FastFood.

---

## Tecnologias Utilizadas

- **.NET 8** - Framework principal da aplicação
- **PostgreSQL** - Banco de dados relacional
- **Entity Framework Core** - ORM para acesso a dados
- **Docker** - Containerização da aplicação
- **Kubernetes** - Orquestração de containers
- **Helm** - Gerenciamento de pacotes Kubernetes
- **GitHub Actions** - CI/CD e pipelines de automação
- **SonarCloud** - Análise de qualidade de códigos

---

## Estrutura do Projeto

```
soat.eleven.fastfood.gestaoprodutos/
├── src/
│   ├── Soat.Eleven.FastFood.GestaoProdutos.Adapter.WebApi/  # Camada de API (Endpoints REST)
│   │   ├── Configuration/                   # Configurações da aplicação
│   │   ├── Endpoints/                       # Definição dos endpoints REST
│   │   └── Program.cs                       # Ponto de entrada da aplicação
│   │
│   ├── Soat.Eleven.FastFood.GestaoProdutos.Application/   # Camada de Aplicação
│   │   └── Controllers/                     # Controllers da aplicação
│   │
│   ├── Soat.Eleven.FastFood.GestaoProdutos.Core/          # Camada de Domínio (Core)
│   │   ├── ConditionRules/                  # Regras de condição
│   │   ├── DTOs/                            # Data Transfer Objects
│   │   ├── Entities/                        # Entidades de domínio (Produto, CategoriaProduto)
│   │   ├── Enums/                           # Enumeradores
│   │   ├── Gateways/                        # Interfaces de gateways
│   │   ├── Interfaces/                      # Interfaces do domínio
│   │   ├── Presenters/                      # Presenters
│   │   └── UseCases/                        # Casos de uso (gestão de produtos e categorias)
│   │
│   └── Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra/ # Camada de Infraestrutura
│       ├── Data/                            # Contexto de dados
│       ├── DataSources/                     # Fontes de dados (ProdutoDataSource, CategoriaProdutoDataSource)
│       ├── EntityModel/                     # Modelos de entidade
│       └── Migrations/                      # Migrações do banco de dados
│
├── Soat.Eleven.FastFood.GestaoProdutos.Tests/  # Projeto de Testes
│   ├── UnitTests/                           # Testes unitários
│   │   ├── Entities/                        # Testes das entidades
│   │   ├── UseCases/                        # Testes dos casos de uso
│   │   └── DTOs/                            # Testes dos DTOs
│   ├── IntegrationTests/                    # Testes de integração
│   │   ├── DataSources/                     # Testes dos DataSources
│   │   └── Endpoints/                       # Testes dos endpoints da API
│   └── Helpers/                             # Classes auxiliares para testes
│
├── helm/
│   └── fastfood-chart/                      # Chart Helm para deploy no Kubernetes
│       ├── templates/                       # Templates Kubernetes
│       └── values.yaml                      # Valores de configuração
│
├── docker-compose.yml                       # Configuração Docker Compose
├── Dockerfile                               # Dockerfile da aplicação
└── README.md
```

---

## Funcionalidades do Microsserviço

### Gestão de Categorias de Produtos
- ✅ Listar categorias de produtos ativas
- ✅ Listar todas as categorias (incluindo inativas)
- ✅ Buscar categoria por ID
- ✅ Criar nova categoria de produto
- ✅ Atualizar categoria existente
- ✅ Desativar categoria
- ✅ Reativar categoria

### Gestão de Produtos
- ✅ Listar produtos por categoria
- ✅ Listar todos os produtos
- ✅ Buscar produto por ID
- ✅ Criar novo produto
- ✅ Atualizar produto existente
- ✅ Desativar produto
- ✅ Reativar produto
- ✅ Gerenciar preços e descrições

---

## Testes Automatizados

O projeto possui uma cobertura completa de testes unitários e de integração, conforme descrito em [Soat.Eleven.FastFood.GestaoProdutos.Tests/README.md](src/Soat.Eleven.FastFood.GestaoProdutos.Tests/README.md).

### Estrutura dos Testes

- **Testes Unitários** - Testam as entidades, casos de uso e DTOs isoladamente
- **Testes de Integração** - Testam os DataSources e endpoints da API
- **Helpers** - Classes auxiliares para criar objetos de teste e configurar o ambiente

### Tecnologias de Teste

- **xUnit** - Framework de testes
- **FluentAssertions** - Assertions mais expressivas
- **NSubstitute** - Mock framework
- **Entity Framework InMemory** - Banco em memória para testes
- **ASP.NET Core Testing** - Testes de integração da API

---

## Deploy e Infraestrutura

### Docker

Para executar a aplicação localmente com Docker:

```bash
docker-compose up -d
```

### Kubernetes com Helm

O projeto inclui um chart Helm em `helm/fastfood-chart/` para deploy no Kubernetes.

Para instalar o chart:

```bash
helm install fastfood-pedido ./helm/fastfood-chart
```

### CI/CD com GitHub Actions

O projeto utiliza **GitHub Actions** para automação de:
- Build e testes automatizados
- Análise de código com SonarCloud
- Build de imagens Docker
- Deploy automatizado no Kubernetes

### Qualidade de Código com SonarCloud

O projeto está integrado ao **SonarCloud** para análise contínua de qualidade de código. As seguintes métricas são monitoradas:

- **Cobertura de Código** - Mínimo de **80%** de cobertura de testes
- **Code Smells** - Identificação de possíveis problemas no código
- **Vulnerabilidades** - Análise de segurança
- **Duplicações** - Detecção de código duplicado

O pipeline de CI/CD impede o merge de código que não atinja a cobertura mínima de 80%.

---

## Pré-requisitos

- .NET 8 SDK
- Docker e Docker Compose
- PostgreSQL (ou usar via Docker)
- Kubernetes e Helm (para deploy em cluster)

---

## Como Executar Localmente

1. Clone o repositório:
```bash
git clone https://github.com/seu-usuario/soat.eleven.fastfood.gestaoprodutos.git
```

2. Configure a connection string do PostgreSQL no `appsettings.json`

3. Execute as migrações do banco de dados:
```bash
dotnet ef database update --project src/Soat.Eleven.FastFood.GestaoProdutos.Adapter.Infra
```

4. Execute a aplicação:
```bash
dotnet run --project src/Soat.Eleven.FastFood.GestaoProdutos.Adapter.WebApi
```

---

## Executando os Testes

Para executar todos os testes:

```bash
dotnet test
```

Para executar apenas os testes unitários:

```bash
dotnet test --filter "UnitTests"
```

Para executar apenas os testes de integração:

```bash
dotnet test --filter "IntegrationTests"
```

Para executar com relatório de cobertura:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

---

## Arquitetura

O projeto segue os princípios de **Clean Architecture**, separando as responsabilidades em camadas:

- **API** - Camada de apresentação com endpoints REST
- **Application** - Orquestração dos casos de uso
- **Core** - Regras de negócio e entidades de domínio
- **Infra** - Implementações de infraestrutura e acesso a dados

---

## Licença

Este projeto foi desenvolvido para fins acadêmicos como parte da pós-graduação em Arquitetura de Software da FIAP.

---