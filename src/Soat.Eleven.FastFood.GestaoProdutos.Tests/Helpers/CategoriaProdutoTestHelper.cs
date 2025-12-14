using Soat.Eleven.FastFood.GestaoProdutos.Core.Entities;

namespace Soat.Eleven.FastFood.GestaoProdutos.Tests.Helpers;

public static class CategoriaProdutoTestHelper
{
    public static CategoriaProduto CreateValidCategoriaProduto(
        Guid? id = null,
        string nome = "Categoria Teste",
        string descricao = "Descrição da categoria teste",
        bool ativo = true,
        DateTime? criadoEm = null,
        DateTime? modificadoEm = null)
    {
        var categoria = new CategoriaProduto
        {
            Id = id ?? Guid.NewGuid(),
            Nome = nome,
            Descricao = descricao,
            Ativo = ativo,
            CriadoEm = criadoEm ?? DateTime.UtcNow,
            ModificadoEm = modificadoEm ?? DateTime.UtcNow
        };

        return categoria;
    }

    public static List<CategoriaProduto> CreateCategoriaProdutoList(int count = 3)
    {
        var categorias = new List<CategoriaProduto>();
        
        for (int i = 1; i <= count; i++)
        {
            categorias.Add(CreateValidCategoriaProduto(
                nome: $"Categoria {i}",
                descricao: $"Descrição da categoria {i}",
                ativo: i % 2 == 1 // Alterna entre ativo e inativo
            ));
        }

        return categorias;
    }
}