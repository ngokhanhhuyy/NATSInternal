using System.Collections;
using Avalonia.Data.Converters;
using NATSInternal.Desktop.ViewModels;

namespace NATSInternal.Desktop.Converters;

public class ErrorCollectionToErrorMessageConverter : IValueConverter
{
    #region Methods
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        string exceptionMessage;
        if (value is not ErrorCollection errorCollection)
        {
            exceptionMessage = $"Argument type must be of type {nameof(ErrorCollection)}.";
            throw new ArgumentException(exceptionMessage);
        }

        if (parameter is null)
        {
            exceptionMessage = $"Argument for {nameof(parameter)} type must not be null.";
            throw new ArgumentException(exceptionMessage);
        }

        if (parameter is not string key)
        {
            exceptionMessage = $"Argument for {nameof(parameter)} type must be string.";
            throw new ArgumentException(exceptionMessage);
        }

        ErrorDetail? detail = errorCollection.FirstOrDefault(detail => detail.PropertyPath == key);
        return detail?.Message;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
    #endregion
}