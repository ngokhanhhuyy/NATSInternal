// using FluentValidation;
// using NATSInternal.Core.Common.Extensions;
// using NATSInternal.Core.Common.Localization;
// using NATSInternal.Core.Features.Users;
// using System.Text.RegularExpressions;

// namespace NATSInternal.Core.Common.Validation;

// internal static partial class RuleBuilderOptionsExtensions
// {
//     #region ExtensionMethods
//     extension<T>(IRuleBuilder<T, string?> ruleBuilder)
//     {
//         public IRuleBuilderOptions<T, string?> IsOneOfFieldsToSort<TFieldToSort>() where TFieldToSort : struct, Enum
//         {
//             return ruleBuilder
//                 .Must(sortByFieldName =>
//                 {
//                     if (sortByFieldName is null)
//                     {
//                         return true;
//                     }

//                     List<string> fieldNames = Enum.GetNames<TFieldToSort>().ToList();
//                     return fieldNames.Any(name => name.Equals(sortByFieldName, StringComparison.OrdinalIgnoreCase));
//                 }).WithMessage(ErrorMessages.Invalid);
//         }
        
//         public IRuleBuilderOptions<T, string?> IsValidName()
//         {
//             const string regexPattern = 
//                 "^[A-Za-zÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂẾ" +
//                 "ưăạảấầẩẫậắằẳẵặẹẻẽềềểếỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸỳỵỷỹ]+$";
            
//             return ruleBuilder.Matches(regexPattern);
//         }
        
//         public IRuleBuilderOptions<T, string?> IsValidWebsiteUrl()
//         {
//             return ruleBuilder
//                 .Must((_, url) =>
//                 {
//                     if (url is null)
//                     {
//                         return true;
//                     }

//                     return GetWebsiteUrlRegex().IsMatch(url);
//                 })
//                 .WithMessage(ErrorMessages.Invalid);
//         }

//         public IRuleBuilderOptions<T, string?> IsValidPhoneNumber()
//         {
//             return ruleBuilder
//                 .Must((_, url) =>
//                 {
//                     if (url is null)
//                     {
//                         return true;
//                     }

//                     return GetPhoneNumberRegex().IsMatch(url);
//                 })
//                 .WithMessage(ErrorMessages.Invalid);
//         }
//     }

//     extension<T>(IRuleBuilder<T, string> ruleBuilder)
//     {
//         public IRuleBuilderOptions<T, string> IsValidPassword()
//         {
//             return ruleBuilder
//                 .Length(UserContracts.PasswordMinLength, UserContracts.PasswordMaxLength)
//                 .Matches(@"^[\\x21-\\x7E]+$");
//         }
//     }

//     // public static IRuleBuilderOptions<T, int> IsValidQueryStatsYear<T>(
//     //         this IRuleBuilder<T, int> ruleBuilder) where T : IHasStatsListRequestDto
//     // {
//     //     return ruleBuilder
//     //         .GreaterThanOrEqualTo(1)
//     //         .LessThanOrEqualTo(DateTime.UtcNow.ToApplicationTime().Year);
//     // }
//     //
//     // public static IRuleBuilderOptions<T, int> IsValidQueryStatsMonth<T>(
//     //         this IRuleBuilder<T, int> ruleBuilder) where T : IHasStatsListRequestDto
//     // {
//     //     return ruleBuilder
//     //         .GreaterThanOrEqualTo(1)
//     //         .LessThanOrEqualTo(DateTime.UtcNow.ToApplicationTime().Month)
//     //         .When(dto => dto.MonthYear.Year == DateTime.UtcNow.ToApplicationTime().Year)
//     //         .LessThanOrEqualTo(12)
//     //         .When(dto => dto.MonthYear.Year < DateTime.UtcNow.ToApplicationTime().Year);
//     // }
//     //
    
//     // public static IRuleBuilderOptions<T, List<PhotoCreateOrUpdateRequestDto>> ContainsNoOrOneThumbnail<T>(
//     //         this IRuleBuilder<T, List<PhotoCreateOrUpdateRequestDto>> ruleBuilder)
//     // {
//     //     return ruleBuilder
//     //         .Must((_, photos) => photos.Count(p => p.IsThumbnail) <= 1)
//     //         .WithMessage(ErrorMessages.PhotosCannotContainsMoreThanOneThumbnail
//     //             .Replace("{Photos}", DisplayNames.Photo.ToLower())
//     //             .Replace("{Thumbnail}", DisplayNames.Thumbnail.ToLower()))
//     //         .WithName(DisplayNames.Photo);
//     // }
//     #endregion
// }