using FluentValidation;
using NATSInternal.Core.Common.Extensions;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Features.Users;
using System.Text.RegularExpressions;

namespace NATSInternal.Core.Common.Validation;

internal static partial class RuleBuilderOptionsExtensions
{
    #region ExtensionMethods
    extension<T>(IRuleBuilder<T, DateTime?> ruleBuilder)
    {
        public IRuleBuilderOptions<T, DateTime?> LaterThanDateTime(DateTime comparisonDateTime)
        {
            string errorMessage = ErrorMessages.LaterThan
                .ReplaceComparisonValue(comparisonDateTime.ToVietnameseString());
            return ruleBuilder.GreaterThan(comparisonDateTime).WithMessage(errorMessage);
        }
        
        public IRuleBuilderOptions<T, DateTime?> LaterThanOrEqualToDateTime(DateTime comparisonDateTime)
        {
            string errorMessage = ErrorMessages.GreaterThanOrEqual
                .ReplaceComparisonValue(comparisonDateTime.ToVietnameseString());
            return ruleBuilder
                .GreaterThanOrEqualTo(comparisonDateTime)
                .WithMessage(errorMessage);
        }
        
        public IRuleBuilderOptions<T, DateTime?> EarlierThanOrEqualToNow()
        {
            DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
            string errorMessage = ErrorMessages.EarlierThanOrEqualToNow
                .ReplaceComparisonValue(currentDateTime.ToVietnameseString());
            return ruleBuilder.LessThanOrEqualTo(currentDateTime).WithMessage(errorMessage);
        }
        
        public IRuleBuilderOptions<T, DateTime?> IsValidStatsDateTime()
        {
            return ruleBuilder.Must(dateTime =>
                {
                    if (dateTime != null)
                    {
                        DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
                        if (dateTime > currentDateTime)
                        {
                            return false;
                        }
                    }
                    return true;
                }).WithMessage(_ =>
                {
                    DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
                    return ErrorMessages.EarlierThanOrEqualToNow
                        .ReplaceComparisonValue(currentDateTime.ToVietnameseString());
                })
                .Must(dateTime =>
                {
                    if (dateTime != null)
                    {
                        if (dateTime < MinimumStatsDateTime)
                        {
                            return false;
                        }
                    }

                    return true;
                }).WithMessage(ErrorMessages.LaterThanOrEqual.ReplaceComparisonValue(
                    MinimumStatsDateTime.ToVietnameseString()));
        }
    }

    extension<T>(IRuleBuilder<T, DateOnly> ruleBuilder)
    {
        public IRuleBuilderOptions<T, DateOnly> LaterThanDate(DateOnly comparisonDate)
        {
            string errorMessage = ErrorMessages.GreaterThan
                .ReplaceComparisonValue(comparisonDate.ToVietnameseString());
            return ruleBuilder.GreaterThan(comparisonDate).WithMessage(errorMessage);
        }
    }

    extension<T>(IRuleBuilder<T, DateOnly?> ruleBuilder)
    {
        public IRuleBuilderOptions<T, DateOnly?> LaterThanOrEqualToDate(DateOnly comparisonDate)
        {
            string errorMessage = ErrorMessages.GreaterThanOrEqual
                .ReplaceComparisonValue(comparisonDate.ToVietnameseString());
            return ruleBuilder.GreaterThanOrEqualTo(comparisonDate).WithMessage(errorMessage);
        }
    }

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

    // public static IRuleBuilderOptions<T, int> IsValidQueryStatsYear<T>(
    //         this IRuleBuilder<T, int> ruleBuilder) where T : IHasStatsListRequestDto
    // {
    //     return ruleBuilder
    //         .GreaterThanOrEqualTo(1)
    //         .LessThanOrEqualTo(DateTime.UtcNow.ToApplicationTime().Year);
    // }
    //
    // public static IRuleBuilderOptions<T, int> IsValidQueryStatsMonth<T>(
    //         this IRuleBuilder<T, int> ruleBuilder) where T : IHasStatsListRequestDto
    // {
    //     return ruleBuilder
    //         .GreaterThanOrEqualTo(1)
    //         .LessThanOrEqualTo(DateTime.UtcNow.ToApplicationTime().Month)
    //         .When(dto => dto.MonthYear.Year == DateTime.UtcNow.ToApplicationTime().Year)
    //         .LessThanOrEqualTo(12)
    //         .When(dto => dto.MonthYear.Year < DateTime.UtcNow.ToApplicationTime().Year);
    // }
    //
    
    // public static IRuleBuilderOptions<T, List<PhotoCreateOrUpdateRequestDto>> ContainsNoOrOneThumbnail<T>(
    //         this IRuleBuilder<T, List<PhotoCreateOrUpdateRequestDto>> ruleBuilder)
    // {
    //     return ruleBuilder
    //         .Must((_, photos) => photos.Count(p => p.IsThumbnail) <= 1)
    //         .WithMessage(ErrorMessages.PhotosCannotContainsMoreThanOneThumbnail
    //             .Replace("{Photos}", DisplayNames.Photo.ToLower())
    //             .Replace("{Thumbnail}", DisplayNames.Thumbnail.ToLower()))
    //         .WithName(DisplayNames.Photo);
    // }
    #endregion

    #region PrivateMethods
    private static DateTime MinimumStatsDateTime
    {
        get
        {
            DateTime currentDateTime = DateTime.UtcNow.ToApplicationTime();
            return new(currentDateTime.AddMonths(-1).Year, currentDateTime.AddMonths(-1).Month, 1, 0, 0, 0);
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