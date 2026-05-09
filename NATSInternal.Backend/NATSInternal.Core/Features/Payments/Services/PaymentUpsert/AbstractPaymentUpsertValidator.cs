using FluentValidation;
using JetBrains.Annotations;
using NATSInternal.Core.Common.Contracts;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Common.Validation;

namespace NATSInternal.Core.Features.Payments;

[UsedImplicitly]
internal abstract class AbstractPaymentUpsertValidator<TUpsertRequestDto> : Validator<TUpsertRequestDto>
    where TUpsertRequestDto : AbstractPaymentUpsertRequestDto
{
    #region Constructors
    public AbstractPaymentUpsertValidator(IClock clock)
    {
        RuleFor(dto => dto.StatsDate)
            .IsValidStatsDate(clock.Today)
            .WithName(DisplayNames.StatsDate);
        RuleFor(dto => dto.Amount)
            .GreaterThan(0)
            .WithName(DisplayNames.Amount);
        RuleFor(dto => dto.Note)
            .MaximumLength(HasStatsContracts.NoteMaxLength)
            .WithName(DisplayNames.Note);
    }
    #endregion
}