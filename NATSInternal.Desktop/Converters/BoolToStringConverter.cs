using Avalonia.Data.Converters;

namespace NATSInternal.Desktop.Converters;

public class BoolToStringConverter : IValueConverter
{
    #region Methods
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo cultureInfo)
    {
        if (value is not bool boolValue)
        {
            throw new ArgumentException("Argument type must be if type [bool].");
        }

        bool isParameterExtractable = TryExtractParameter(
            parameter,
            out Dictionary<bool, string> extractedParameters
        );

        if (!isParameterExtractable)
        {
            throw new ArgumentException($"Invalid argument for {nameof(BoolToStringConverter)}.", nameof(value));
        }

        return extractedParameters[boolValue];
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not string stringValue)
        {
            throw new ArgumentException("Argument type must be of type [string].");
        }
        

        bool isParametersExtractable = TryExtractParameter(
            parameter,
            out Dictionary<bool, string> extractedParameters
        );

        if (!isParametersExtractable)
        {
            throw new ArgumentException($"Invalid argument for {nameof(BoolToStringConverter)}.", nameof(value));
        }

        return extractedParameters.First(p => p.Value == stringValue);
    }
    #endregion

    #region PrivateMethods
    private static bool TryExtractParameter(object? parameter, out Dictionary<bool, string> extractedParameter)
    {
        extractedParameter = new()
        {
            { true, true.ToString() },
            { false, false.ToString() },
        };
        
        if (parameter is null)
        {
            return true;
        }

        if (parameter is string stringParamters)
        {
            string[] splittedParameters = stringParamters.Split(", ");
            if (splittedParameters.Length == 2)
            {
                extractedParameter[true] = splittedParameters[0];
                extractedParameter[false] = splittedParameters[1];

                return true;
            }
        }

        if (parameter is string[] { Length: 2 } stringArrayParameters)
        {
            extractedParameter[true] = stringArrayParameters[0];
            extractedParameter[false] = stringArrayParameters[1];

            return true;
        }

        return false;
    }
    #endregion
}