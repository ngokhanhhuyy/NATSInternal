using JetBrains.Annotations;
using FluentValidation;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Features.Photos;

namespace NATSInternal.Core.Features.Supplies;

[UsedImplicitly]
internal class SupplyCreateValidator : AbstractSupplyUpsertValidator<SupplyCreateRequestDto>
{
    #region Constructors
    public SupplyCreateValidator(
        IValidator<SupplyUpsertItemRequestDto> itemValidator,
        IValidator<PhotoUpsertRequestDto> photoValidator,
        IClock clock) : base(itemValidator, photoValidator, clock)
    {
        When(dto => !dto.IsCorrection, () =>
        {
            RuleFor(dto => dto.ShipmentFee)
                .GreaterThanOrEqualTo(0)
                .WithName(DisplayNames.ShipmentFee);
            RuleFor(dto => dto.Items)
                .NotEmpty()
                .WithName(DisplayNames.SupplyItem);
            RuleForEach(dto => dto.Photos).SetValidator(photoValidator, ruleSets: "Create");
        });
    }
    #endregion
}
