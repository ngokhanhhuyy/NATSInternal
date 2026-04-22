using System.Globalization;
using System.Text;

namespace NATSInternal.Domain.Extensions;

internal static class StringExtensions
{
    #region ExtensionMethods
    public static string ToNonDiacritics(this string value)
    {
        string normalizedString = value.Normalize(NormalizationForm.FormD);
        StringBuilder stringBuilder = new();
    
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
    #endregion
}