using Soat.Eleven.FastFood.GestaoProdutos.Core.ConditionRules;

namespace Soat.Eleven.FastFood.GestaoProdutos.Core.Entities;

public class Produto
{
    public Guid Id { get; set; }
    private string nome = string.Empty;

    public string Nome
    {
        get { return nome; }
        set
        {
            Condition.Require(value, "Nome").IsNullOrEmpty();
            nome = value;
        }
    }
    private string sku = string.Empty;

    public string SKU
    {
        get { return sku; }
        set
        {
            Condition.Require(value, "SKU").IsNullOrEmpty();
            sku = value;
        }
    }
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public Guid CategoriaId { get; set; }
    public bool Ativo { get; set; }    
    public string? Imagem { get; set; }
    public DateTime CriadoEm { get; set; }
    public DateTime ModificadoEm { get; set; }

    public CategoriaProduto Categoria { get; set; } = null!;
}
