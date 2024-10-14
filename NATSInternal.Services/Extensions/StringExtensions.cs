using System.Globalization;
using System.Text;

namespace NATSInternal.Services.Extensions;

public static class StringExtensions
{
    public static string ToNullIfEmpty(this string value)
    {
        return string.IsNullOrWhiteSpace(value?.Trim()) ? null : value.Trim();
    }

    public static string CapitalizeFirstLetter(this string value)
    {
        string result = string.Empty;
        for (int i = 0; i < value.Length; i++) {
            if (i == 0 || value[i - 1] == ' ' || value[i - 1] == '\n') {
                result += value[i].ToString().ToUpper();
                continue;
            }
            result += value[i];
        }
        return result;
    }

    public static string SnakeCaseToPascalCase(this string snakeCaseString)
    {
        string[] snakeCaseSegments = snakeCaseString.Split("_");
        List<string> pascalCaseSegments = new List<string>();
        foreach (string snakeCaseSegment in snakeCaseSegments)
        {
            if (snakeCaseSegment.Length == 0)
            {
                pascalCaseSegments.Add(string.Empty);
                continue;
            }
            string pascalCaseSegment = snakeCaseSegment[0].ToString().ToUpper();
            if (snakeCaseSegment.Length > 1)
            {
                pascalCaseSegment += snakeCaseSegment[1..];
            }
            pascalCaseSegment = pascalCaseSegment
                .Replace("Username", "UserName")
                .Replace("Datetime", "CreatedDateTime");
            pascalCaseSegments.Add(pascalCaseSegment);
        }
        return string.Concat();
    }

    public static string SnakeCaseToCamelCase(this string snakeCaseString)
    {
        string[] snakeCaseSegments = snakeCaseString.Split("_");
        List<string> pascalCaseSegments = new List<string>();
        foreach (string snakeCaseSegment in snakeCaseSegments)
        {
            if (snakeCaseSegment.Length == 0)
            {
                pascalCaseSegments.Add(string.Empty);
                continue;
            }
            string pascalCaseSegment = snakeCaseSegment[0].ToString().ToUpper();
            if (snakeCaseSegment.Length > 1)
            {
                pascalCaseSegment += snakeCaseSegment[1..];
            }
            pascalCaseSegments.Add(pascalCaseSegment);
        }
        return string.Concat(pascalCaseSegments)
            .Replace("Fullname", "FullName")
            .Replace("Username", "UserName")
            .Replace("Datetime", "DateTime");
    }

    public static string PascalCaseToSnakeCase(this string pascalCaseString)
    {
        List<string> snakeCaseWords = new List<string>();

        if (pascalCaseString.Length == 0)
        {
            return string.Empty;
        }

        if (pascalCaseString.Length == 1)
        {
            return pascalCaseString.ToUpper();
        }

        int startingIndex = 0;
        for (int i = 1; i < pascalCaseString.Length; i++)
        {
            char character = pascalCaseString[i];
            bool isUpper = char.IsUpper(character);
            bool isTheEnd = i == pascalCaseString.Length - 1;

            if (
                    isTheEnd ||
                    (
                        isUpper &&
                        (
                            char.IsLower(pascalCaseString[i - 1]) ||
                            char.IsLower(pascalCaseString[i + 1])
                        )
                    )
                )
            {
                string snakeCaseWord = pascalCaseString[startingIndex..i].ToLower();
                if (isTheEnd)
                {
                    snakeCaseWord += char.ToLower(character).ToString();
                }

                snakeCaseWords.Add(snakeCaseWord);
                startingIndex = i;
            }
        }

        return string
            .Join("_", snakeCaseWords)
            .Replace("full_name", "fullname")
            .Replace("user_name", "username")
            .Replace("date_time", "datetime");
    }

    public static string ToNonDiacritics(this string value)
    {
        string normalizedString = value.Normalize(NormalizationForm.FormD);
        StringBuilder stringBuilder = new StringBuilder();
    
        foreach (Rune rune in normalizedString.EnumerateRunes())
        {
            var unicodeCategory = Rune.GetUnicodeCategory(rune);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(rune);
            }
        }
    
        return stringBuilder
            .ToString()
            .Normalize(NormalizationForm.FormC)
            .Replace('đ', 'd')
            .Replace('Đ', 'D')
            .Replace('Ð', 'D');
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
}