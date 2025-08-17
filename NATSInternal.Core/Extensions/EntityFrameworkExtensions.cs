namespace NATSInternal.Core.Extensions;

internal static class EntityFrameworkExtensions
{
    #region ExtensionMethods
    public static PropertyBuilder<TData> HasJsonConversion<TData>(
            this PropertyBuilder<TData> propertyBuilder,
            JsonSerializerOptions serializerOptions)
        where TData : class
    {
        return propertyBuilder.HasConversion(
            data => JsonSerializer.Serialize(data, serializerOptions),
            json => JsonSerializer.Deserialize<TData>(json, serializerOptions)!
        );
    }
    #endregion
}
