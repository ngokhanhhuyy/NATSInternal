using FluentValidation;
using NATSInternal.Core.Common.Extensions;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Features.Users;
using System.Text.RegularExpressions;

namespace NATSInternal.Core.Common.Validation;

internal static partial class StringRuleBuilderOptionsExtensions
{
    #region ExtensionMethods
    extension<T>(IRuleBuilder<T, string?> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string?> IsOneOfFieldsToSort<TFieldToSort>() where TFieldToSort : struct, Enum
        {
            return ruleBuilder
                .Must(sortByFieldName =>
                {
                    if (sortByFieldName is null)
                    {
                        return true;
                    }

                    List<string> fieldNames = Enum.GetNames<TFieldToSort>().ToList();
                    return fieldNames.Any(name => name.Equals(sortByFieldName, StringComparison.OrdinalIgnoreCase));
                }).WithMessage(ErrorMessages.Invalid);
        }
        
        public IRuleBuilderOptions<T, string?> IsValidName()
        {
            const string regexPattern = 
                "^[A-Za-zÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾ" +
                "ưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸỳỵỷỹ]+$";
            
            return ruleBuilder.Matches(regexPattern);
        }
        
        public IRuleBuilderOptions<T, string?> IsValidWebsiteUrl()
        {
            return ruleBuilder
                .Must((_, url) =>
                {
                    if (url is null)
                    {
                        return true;
                    }

                    return GetWebsiteUrlRegex().IsMatch(url);
                })
                .WithMessage(ErrorMessages.Invalid);
        }

        public IRuleBuilderOptions<T, string?> IsValidPhoneNumber()
        {
            return ruleBuilder
                .Must((_, url) =>
                {
                    if (url is null)
                    {
                        return true;
                    }

                    return GetPhoneNumberRegex().IsMatch(url);
                })
                .WithMessage(ErrorMessages.Invalid);
        }
    }

    extension<T>(IRuleBuilder<T, string> ruleBuilder)
    {
        public IRuleBuilderOptions<T, string> IsValidPassword()
        {
            return ruleBuilder
                .Length(UserContracts.PasswordMinLength, UserContracts.PasswordMaxLength)
                .Matches(@"^[\\x21-\\x7E]+$");
        }
    }
    #endregion

    #region StaticMethods
    [GeneratedRegex(@"^((http|https)://)?(www\.)?([A-Za-z0-9]+(-[A-Za-z0-9]+)*)(\.([A-Za-z0-9]+(-[A-Za-z0-9]+)*))+(\/.*)?$")]
    private static partial Regex GetWebsiteUrlRegex();

    [GeneratedRegex(@"^\+?[0-9]+$")]
    private static partial Regex GetPhoneNumberRegex();
    #endregion
}