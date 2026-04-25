using FluentValidation;
using NATSInternal.Core.Common.Extensions;
using NATSInternal.Core.Common.Localization;

namespace NATSInternal.Core.Common.Validation;

internal static class DateOnlyRuleBuilderOptionsExtensions
{
    #region ExtensionMethods
    extension<T>(IRuleBuilder<T, DateOnly?> ruleBuilder)
    {
        public IRuleBuilderOptions<T, DateOnly?> EarlierThanDate(DateOnly comparisonDate)
        {
            string comparisonDateAsString = comparisonDate.ToVietnameseString();
            string errorMessage = ErrorMessages.EarlierThan.ReplaceComparisonValue(comparisonDateAsString);
            
            return ruleBuilder.LessThan(comparisonDate).WithMessage(errorMessage);
        }

        public IRuleBuilderOptions<T, DateOnly?> EarlierOrEqualToDate(DateOnly comparisonDate)
        {
            string comparisonDateAsString = comparisonDate.ToVietnameseString();
            string errorMessage = ErrorMessages.EarlierThanOrEqual.ReplaceComparisonValue(comparisonDateAsString);

            return ruleBuilder.LessThanOrEqualTo(comparisonDate).WithMessage(errorMessage);
        }

        public IRuleBuilderOptions<T, DateOnly?> LaterThanDate(DateOnly comparisonDate)
        {
            string comparisonDateAsString = comparisonDate.ToVietnameseString();
            string errorMessage = ErrorMessages.LaterThan.ReplaceComparisonValue(comparisonDateAsString);

            return ruleBuilder.GreaterThan(comparisonDate).WithMessage(errorMessage);
        }

        public IRuleBuilderOptions<T, DateOnly?> LaterThanOrEqualToDate(DateOnly comparisonDate)
        {
            string comparisonDateAsString = comparisonDate.ToVietnameseString();
            string errorMessage = ErrorMessages.LaterThanOrEqual.ReplaceComparisonValue(comparisonDateAsString);

            return ruleBuilder.GreaterThanOrEqualTo(comparisonDate).WithMessage(errorMessage);
        }
    }
    #endregion
}