using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;

namespace Soat.Eleven.FastFood.GestaoProdutos.Adapter.WebApi.Configuration;

public static class SwaggerConfiguration
{
    public static void AddSwaggerConfiguration(this IServiceCollection services)
    {
        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "FastFood Api - Gestão de Produtos",
                Description = @"
Projeto acadêmico desenvolvido para a disciplina de Arquitetura de Software (FIAP - Pós-graduação)

# Autenticação e Autorização

- **JWT Bearer Token**: Todas as rotas (exceto geração de token de atendimento) exigem autenticação via JWT.
- **Perfis de Usuário**:
    - `Administrador`: gerenciamento de categorias, produtos e operações de cozinha (preparação de pedidos).
    - `Cliente`: acesso aos próprios dados e pode cancelar próprios pedidos.
    - `ClienteTotem`: pode criar e pagar pedidos via totem.
    - `Commom`: acesso básico para visualização de dados gerais.
- **Como obter token**:
    1. Obtém um token JWT autenticando-se via endpoint de login

- **Usuário e senha padrão (admin)**:
    - Usuário: `sistema@fastfood.com`
    - Senha: `Senha@123`

# Ordem recomendada de execução dos endpoints

1. **Cadastro de Categorias e Produtos (obrigatório - apenas admin)**
    - `/api/Categoria` (POST) — Criação de categoria (PolicyRole.Administrador)
    - `/api/Produto` (POST) — Criação de produto (PolicyRole.Administrador)
    - `/api/Produto` (GET) — Listagem de produtos (requer auth)
    - `/api/Categoria` (GET) — Listagem de categorias (requer auth)

> **Importante:** Sempre envie o token JWT no header `Authorization: Bearer {token}` para acessar os endpoints protegidos.
> 
>
"
            });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n " +
                    "Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    public static void UseSwaggerConfiguration(this WebApplication applicationBuilder)
    {
        applicationBuilder.UseSwagger();
        applicationBuilder.UseSwaggerUI();
    }
}
