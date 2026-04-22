using Avalonia.Data.Converters;

namespace NATSInternal.Desktop.Converters;

public static class OneWayConverters
{
    #region StaticMethods
    public static readonly IValueConverter StringToBoolConverter =
        new FuncValueConverter<string, bool>(v => !string.IsNullOrEmpty(v));

    public static readonly IValueConverter BoolInvertConverter = new FuncValueConverter<bool, bool>(v => !v);
    #endregion
}