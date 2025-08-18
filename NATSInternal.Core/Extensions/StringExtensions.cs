using System.Globalization;

namespace NATSInternal.Core.Extensions;

public static class StringExtensions
{
    #region ExtensionMethods
    public static string? ToNullIfEmpty(this string? value)
    {
        return string.IsNullOrWhiteSpace(value?.Trim()) ? null : value.Trim();
    }

    public static string ReplaceResourceName(this string value, string resouceDisplayName)
    {
        return value.Replace("{ResourceName}", resouceDisplayName);
    }

    public static string ReplacePropertyName(this string value, string propertyDisplayName)
    {
        return value.Replace("{PropertyName}", propertyDisplayName);
    }

    public static string ReplaceAttemptedValue(this string value, string attemptedValue)
    {
        return value.Replace("{AttemptedValue}", attemptedValue);
    }

    public static string ReplaceComparisonValue(this string value, string comparisonValue)
    {
        return value.Replace("{ComparisonValue}", comparisonValue);
    }
    #endregion
}