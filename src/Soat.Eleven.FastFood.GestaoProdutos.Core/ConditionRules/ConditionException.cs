namespace Soat.Eleven.FastFood.GestaoProdutos.Core.ConditionRules;

public abstract class ConditionException<T>
{
    internal T Target { get; set; }
    internal string ArgumentName { get; set; }

    private protected ConditionException(T target, string argumentName)
    {
        Target = target;
        ArgumentName = argumentName;
    }
}
