using Soat.Eleven.FastFood.GestaoProdutos.Core.ConditionRules;

namespace Soat.Eleven.FastFood.GestaoProdutos.Core.Entities;

public class DescontoProduto
{
    public Guid DescontoProdutoId { get; set; }
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
    public Guid ProdutoId { get; set; }
    private decimal valor;

    public decimal Valor
    {
        get { return valor; }
        set 
        {
            Condition.Require(value, "Valor").IsGreaterThanOrEqualTo(0);
            valor = value; 
        }
    }

    public DateTime DataInicio { get; set; }
    public DateTime? DataFim { get; set; }
    public bool Ativo { get; set; }
    public DateTime CriadoEm { get; set; }
    public Produto Produto { get; set; } = null!;
}
