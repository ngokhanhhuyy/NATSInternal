namespace NATSInternal.Blazor.Models;

public class BaseModel<TModel>
{
    public string DisplayNameFor(Expression<Func<TModel, object>> propertySelector)
    {
        string propertyName;
        if (propertySelector.Body is MemberExpression memberExpression)
        {
            propertyName = memberExpression.Member.Name;
        }
        // For cases where the property is cast to object (e.g., value types)
        else if (propertySelector.Body is UnaryExpression unaryExpression &&
            unaryExpression.Operand is MemberExpression operandExpression)
        {
            propertyName = operandExpression.Member.Name;
        }
        else
        {
            throw new ArgumentException("Invalid property expression.");
        }

        PropertyInfo property = typeof(TModel).GetProperty(propertyName);
        DisplayAttribute displayAttribute = property.GetCustomAttribute<DisplayAttribute>();
        return displayAttribute?.Name ?? property.Name;
    }
}
