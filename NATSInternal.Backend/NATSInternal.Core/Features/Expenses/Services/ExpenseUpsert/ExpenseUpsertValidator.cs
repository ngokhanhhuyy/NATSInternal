using FluentValidation;
using JetBrains.Annotations;
using NATSInternal.Core.Common.Contracts;
using NATSInternal.Core.Common.Localization;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Common.Validation;
using NATSInternal.Core.Features.Photos;

namespace NATSInternal.Core.Features.Expenses;

[UsedImplicitly]
internal class ExpenseUpsertValidator : Validator<ExpenseUpsertRequestDto>
{
    #region Constructors
    public ExpenseUpsertValidator(IValidator<PhotoUpsertRequestDto> photoValidator, IClock clock)
    {
        RuleFor(dto => dto.StatsDate)
            .IsValidStatsDate(clock.Today)
            .WithName(DisplayNames.StatsDate);
        RuleFor(dto => dto.Amount)
            .GreaterThan(0)
            .WithName(DisplayNames.Amount);
        RuleFor(dto => dto.Type)
            .IsInEnum()
            .WithName(DisplayNames.ExpenseType);
        RuleFor(dto => dto.Note)
            .MaximumLength(HasStatsContracts.NoteMaxLength)
            .WithName(DisplayNames.Note);

        RuleSet("Create", () =>
        {
            RuleForEach(dto => dto.Photos).SetValidator(photoValidator, ruleSets: "Create");
        });

        RuleSet("Update", () =>
        {
            RuleForEach(dto => dto.Photos).SetValidator(photoValidator, ruleSets: "CreateAndUpdate");
        });
    }
    #endregion
}