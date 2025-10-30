using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Routing;

namespace NATSInternal.Api.Configurations;

public partial class KebabParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value is null)
        {
            return null;
        }

        return GetPascalCaseRegex().Replace(value.ToString()!, "$1-$2").ToLower();
    }

    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex GetPascalCaseRegex();
}