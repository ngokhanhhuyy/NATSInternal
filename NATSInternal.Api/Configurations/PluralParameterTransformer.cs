using System.Text.RegularExpressions;
using Humanizer;
using NATSInternal.Api.Controllers;

namespace NATSInternal.Api.Configurations;

public partial class PluralParameterTransformer : IOutboundParameterTransformer
{
    #region StaticConstructors
    static PluralParameterTransformer()
    {
        string[] pluralizationIgnoredControllerNames = new[]
        {
            nameof(AuthenticationController),
            nameof(MetadataController)
        };

        PluralizationIgnoredNames = pluralizationIgnoredControllerNames
            .Select(name => GetControllerNameRegex().Replace(name, ""));
    }
    #endregion
    
    #region Methods
    public string? TransformOutbound(object? value)
    {
        string? stringValue = value?.ToString();
        if (stringValue is null)
        {
            return null;
        }

        if (PluralizationIgnoredNames.Contains(stringValue, StringComparer.OrdinalIgnoreCase))
        {
            return stringValue;
        }

        return stringValue.Pluralize();
    }
    #endregion
    
    #region StaticProperties
    private static IEnumerable<string> PluralizationIgnoredNames { get; }
    #endregion
    
    #region StaticMethods
    [GeneratedRegex("(Controller)$")]
    private static partial Regex GetControllerNameRegex();
    #endregion
}