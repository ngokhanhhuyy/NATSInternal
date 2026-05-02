using JetBrains.Annotations;
using FluentValidation;
using NATSInternal.Core.Common.Contracts;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Common.Validation;
using NATSInternal.Core.Features.Photos;

namespace NATSInternal.Core.Features.Supplies;

[UsedImplicitly]
internal class SupplyUpsertValidator : Validator<SupplyUpsertRequestDto>
{
    #region Constructors
    public SupplyUpsertValidator(
        IValidator<SupplyUpsertItemRequestDto> itemValidator,
        IValidator<PhotoUpsertRequestDto> photoValidator,
        IClock clock)
    {
        RuleFor(dto => dto.StatsDateTime)
            .IsValidStatsDateTime(clock.Now)
            .WithName(DisplayNames.StatsDateTime);
        RuleFor(dto => dto.ShipmentFee)
            .GreaterThanOrEqualTo(0)
            .WithName(DisplayNames.ShipmentFee);
        RuleFor(dto => dto.Note)
            .MaximumLength(HasStatsContracts.NoteMaxLength)
            .WithName(DisplayNames.Note);
        RuleFor(dto => dto.Items)
            .NotEmpty()
            .WithName(DisplayNames.SupplyItem);
        RuleForEach(dto => dto.Items)
            .SetValidator(itemValidator);

        RuleSet("Create", () =>
        {
            RuleForEach(dto => dto.Photos).SetValidator(photoValidator, ruleSets: "Create");
        });

        RuleSet("CreateAndUpdate", () =>
        {
            RuleForEach(dto => dto.Photos).SetValidator(photoValidator, ruleSets: "CreateAndUpdate");
        });
    #endregion
    }
}
