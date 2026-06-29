using FluentValidation;
using JetBrains.Annotations;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Common.Validation;

namespace NATSInternal.Core.Features.Supplies;

[UsedImplicitly]
internal class SupplyListValidator : Validator<SupplyListRequestDto>
{
    #region Constructors
    public SupplyListValidator(IClock clock)
    {
        Include(new HasStatsListValidator<SupplyListRequestDto, SupplyListRequestDto.FieldToSort>(clock));
        
        RuleFor(dto => dto.ProductId)
            .GreaterThan(0)
            .WithName(DisplayNames.ProductId);
    }
    #endregion
}