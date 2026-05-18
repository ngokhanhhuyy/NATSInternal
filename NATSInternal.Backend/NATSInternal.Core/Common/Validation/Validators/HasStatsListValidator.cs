using FluentValidation;
using NATSInternal.Core.Common.Time;
using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Localization;

namespace NATSInternal.Core.Common.Validation;

internal class HasStatsListValidator<TListRequestDto, TFieldToSort> : Validator<TListRequestDto>
    where TListRequestDto : IHasStatsListRequestDto
    where TFieldToSort : struct, Enum
{
    #region Constructors
    public HasStatsListValidator(IClock clock)
    {
        Include(new ListValidator<TListRequestDto, TFieldToSort>());

        DateOnly today = clock.Today;
        RuleFor(dto => dto.StatsYear)
            .GreaterThanOrEqualTo(0)
            .LessThanOrEqualTo(today.Year)
            .WithName(DisplayNames.Year);
            
        RuleFor(dto => dto.StatsMonth)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(today.Month)
            .When(dto => dto.StatsYear == today.Year)
            .WithName(DisplayNames.Month);
            
        RuleFor(dto => dto.StatsMonth)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(12)
            .When(dto => dto.StatsYear < today.Year)
            .WithName(DisplayNames.Month);
    }
    #endregion
}
