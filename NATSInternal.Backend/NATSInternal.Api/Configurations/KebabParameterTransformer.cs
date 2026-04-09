using System.Text.RegularExpressions;

namespace NATSInternal.Api.Configurations;

public partial class KebabParameterTransformer : IOutboundParameterTransformer
{
    #region Methods
    public string? TransformOutbound(object? value)
    {
        if (value?.ToString() is null)
        {
            return null;
        }

        return GetPascalCaseRegex().Replace(value.ToString()!, "$1-$2").ToLower();
    }
    #endregion

    #region StaticMethods
    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex GetPascalCaseRegex();
    #endregion
}