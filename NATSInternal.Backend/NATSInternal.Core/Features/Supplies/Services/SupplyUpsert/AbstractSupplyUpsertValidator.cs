using JetBrains.Annotations;
using FluentValidation;
using NATSInternal.Core.Common.Contracts;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Common.Validation;
using NATSInternal.Core.Features.Photos;

namespace NATSInternal.Core.Features.Supplies;

[UsedImplicitly]
internal abstract class AbstractSupplyUpsertValidator<TUpsertRequestDto> : Validator<TUpsertRequestDto>
    where TUpsertRequestDto : AbstractSupplyUpsertRequestDto
{
    #region Constructors
    protected AbstractSupplyUpsertValidator(
        IValidator<SupplyUpsertItemRequestDto> itemValidator,
        IValidator<PhotoUpsertRequestDto> photoValidator,
        IClock clock)
    {
        RuleFor(dto => dto.StatsDate)
            .IsValidStatsDate(clock.Today)
            .WithName(DisplayNames.StatsDate);
        RuleFor(dto => dto.Note)
            .MaximumLength(HasStatsContracts.NoteMaxLength)
            .WithName(DisplayNames.Note);
        RuleForEach(dto => dto.Items)
            .SetValidator(itemValidator);

        RuleSet("Create", () =>
        {
        });

        RuleSet("Update", () =>
        {
            RuleForEach(dto => dto.Photos).SetValidator(photoValidator, ruleSets: "CreateAndUpdate");
        });
    #endregion
    }
}
