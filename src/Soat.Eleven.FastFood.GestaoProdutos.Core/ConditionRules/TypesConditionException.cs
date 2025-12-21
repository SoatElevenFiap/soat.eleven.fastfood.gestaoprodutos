using System.ComponentModel.DataAnnotations;

namespace Soat.Eleven.FastFood.GestaoProdutos.Core.ConditionRules;

public static class TypesConditionException
{
    #region String
    public static void IsNullOrEmpty(this ConditionException<string> condition)
    {
        if (string.IsNullOrEmpty(condition.Target))
        {
            throw new ArgumentException($"{condition.ArgumentName} cannot be Null or Empty");
        }
    }

    public static void IsEmail(this ConditionException<string> condition)
    {
        var email = new EmailAddressAttribute();

        if (!email.IsValid(condition.Target) || string.IsNullOrEmpty(condition.Target))
        {
            throw new ArgumentException($"{condition.ArgumentName} is not valid email");
        }
    }

    public static void MinimumCharacter(this ConditionException<string> condition, int min)
    {
        if (condition.Target.Length >= min)
        {
            throw new ArgumentException($"{condition.ArgumentName} cannot be less than {min}");
        }
    }

    public static void MaximumCharacter(this ConditionException<string> condition, int max)
    {
        if (condition.Target.Length <= max)
        {
            throw new ArgumentException($"{condition.ArgumentName} cannot be greater than {max}");
        }
    }
    #endregion

    #region Int
    public static void IsGreaterThan(this ConditionException<int> condition, int target)
    {
        if (condition.Target > target)
        {
            throw new ArgumentException($"{condition.ArgumentName} must be greater than {target}");
        }
    }
    public static void IsLessThan(this ConditionException<int> condition, int target)
    {
        if (condition.Target < target)
        {
            throw new ArgumentException($"{condition.ArgumentName} must be less than {target}");
        }
    }
    public static void IsGreaterThanOrEqualTo(this ConditionException<int> condition, int target)
    {
        if (condition.Target >= target)
        {
            throw new ArgumentException($"{condition.ArgumentName} must be greater or equal than {target}");
        }
    }
    public static void IsLessThanOrEqualTo(this ConditionException<int> condition, int target)
    {
        if (condition.Target < target)
        {
            throw new ArgumentException($"{condition.ArgumentName} must be less or equal than {target}");
        }
    }
    #endregion

    #region Decimal
    public static void IsGreaterThan(this ConditionException<decimal> condition, decimal target)
    {
        if (condition.Target > target)
        {
            throw new ArgumentException($"{condition.ArgumentName} must be greater than {target}");
        }
    }
    public static void IsLessThan(this ConditionException<decimal> condition, decimal target)
    {
        if (condition.Target < target)
        {
            throw new ArgumentException($"{condition.ArgumentName} must be less than {target}");
        }
    }
    public static void IsGreaterThanOrEqualTo(this ConditionException<decimal> condition, decimal target)
    {
        if (condition.Target >= target)
        {
            throw new ArgumentException($"{condition.ArgumentName} must be greater or equal than {target}");
        }
    }
    public static void IsLessThanOrEqualTo(this ConditionException<decimal> condition, decimal target)
    {
        if (condition.Target < target)
        {
            throw new ArgumentException($"{condition.ArgumentName} must be less or equal than {target}");
        }
    }
    #endregion
}
