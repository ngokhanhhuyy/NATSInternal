using FluentValidation;
using NATSInternal.Core.Common.Localization;

namespace NATSInternal.Core.Common.Validation;

internal static partial class CollectionRuleBuilderOptionsExtensions
{
    #region ExtensionMethods
    extension<T, TItem>(IRuleBuilder<T, List<TItem>?> ruleBuilder) where TItem : class
    {
        public IRuleBuilderOptions<T, List<TItem>?> Unique<TProperty>(Func<TItem, TProperty> propertySelector)
        {
            return ruleBuilder
                .Must((items) =>
                {
                    if (items is null)
                    {
                        return true;
                    }

                    return items.Select(propertySelector).Count() > items.Count;
                })
                .WithMessage(ErrorMessages.Duplicated);
        }
    }
    #endregion
}