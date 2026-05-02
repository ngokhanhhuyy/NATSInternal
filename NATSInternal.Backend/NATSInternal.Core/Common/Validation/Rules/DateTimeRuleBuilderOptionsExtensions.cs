using FluentValidation;
using NATSInternal.Core.Common.Extensions;
using NATSInternal.Core.Common.Localization;

namespace NATSInternal.Core.Common.Validation;

internal static class DateTimeRuleBuilderOptionsExtensions
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
        
        public IRuleBuilderOptions<T, DateTime?> IsValidStatsDateTime(DateTime currentDateTime)
        {
            DateTime minimumStatsDateTime = GetMinimumStatsDateTime(currentDateTime);
            return ruleBuilder.Must(dateTime =>
                {
                    if (dateTime != null)
                    {
                        if (dateTime > currentDateTime)
                        {
                            return false;
                        }
                    }
                    return true;
                })
                .WithMessage(_ =>
                {
                    return ErrorMessages.EarlierThanOrEqualToNow
                        .ReplaceComparisonValue(currentDateTime.ToVietnameseString());
                })
                .Must(dateTime =>
                {
                    if (dateTime != null)
                    {
                        if (dateTime < minimumStatsDateTime)
                        {
                            return false;
                        }
                    }

                    return true;
                }).WithMessage(ErrorMessages.LaterThanOrEqual.ReplaceComparisonValue(
                    minimumStatsDateTime.ToVietnameseString()));
        }
    }
    #endregion

    #region PrivateMethods
    private static DateTime GetMinimumStatsDateTime(DateTime currentDateTime)
    {
        return new(currentDateTime.AddMonths(-1).Year, currentDateTime.AddMonths(-1).Month, 1, 0, 0, 0);
    }
    #endregion
}