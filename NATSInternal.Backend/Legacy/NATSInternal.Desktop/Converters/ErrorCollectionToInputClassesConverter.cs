using Avalonia.Data.Converters;
using NATSInternal.Desktop.ViewModels;

namespace NATSInternal.Desktop.Converters;

public class ErrorCollectionToInputClassesConverter : IValueConverter
{
    #region Methods
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not ErrorCollection errorCollection || parameter is null)
        {
            return null;
        }

        if (parameter is not string key)
        {
            return null;
        }

        string? message = errorCollection
            .Where(detail => detail.PropertyPath == key)
            .Select(detail => detail.Message)
            .FirstOrDefault();
        
        return string.IsNullOrEmpty(message) ? null : "Invalid";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
    #endregion
}