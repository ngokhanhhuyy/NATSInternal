namespace NATSInternal.Core.Common.Extensions;

public static class StringExtensions
{
    #region Extensions
    extension(string? value)
    {
        public string? ToNullIfEmptyOrWhiteSpace()
        {
            return string.IsNullOrWhiteSpace(value?.Trim()) ? null : value.Trim();
        }
    }

    extension(string value)
    {
        public string ReplaceResourceName(string resouceDisplayName)
        {
            return value.Replace("{ResourceName}", resouceDisplayName);
        }

        public string ReplacePropertyName(string propertyDisplayName)
        {
            return value.Replace("{PropertyName}", propertyDisplayName);
        }

        public string ReplaceAttemptedValue(string attemptedValue)
        {
            return value.Replace("{AttemptedValue}", attemptedValue);
        }

        public string ReplaceComparisonValue(string comparisonValue)
        {
            return value.Replace("{ComparisonValue}", comparisonValue);
        }
    }
    #endregion
}