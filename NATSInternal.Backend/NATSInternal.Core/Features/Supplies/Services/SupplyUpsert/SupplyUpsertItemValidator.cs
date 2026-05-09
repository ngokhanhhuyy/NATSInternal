using JetBrains.Annotations;
using FluentValidation;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Validation;

namespace NATSInternal.Core.Features.Supplies;

[UsedImplicitly]
internal class SupplyItemValidator : Validator<SupplyUpsertItemRequestDto>
{
    #region Constructors
    public SupplyItemValidator()
    {
        RuleFor(dto => dto.AmountPerUnit)
            .GreaterThanOrEqualTo(0)
            .WithName(DisplayNames.Amount);
        RuleFor(dto => dto.Quantity)
            .GreaterThan(0)
            .WithName(DisplayNames.SuppliedQuatity);
    }
    #endregion
}
