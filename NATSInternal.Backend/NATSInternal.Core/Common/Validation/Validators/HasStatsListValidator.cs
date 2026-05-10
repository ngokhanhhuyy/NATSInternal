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

        RuleFor(dto => dto.StatsMonthYear)
            .ChildRules(cr =>
            {
                #nullable disable
                DateOnly today = clock.Today;

                cr.RuleFor(my => my.Year)
                    .GreaterThanOrEqualTo(0)
                    .LessThanOrEqualTo(today.Year)
                    .WithName(DisplayNames.Year);
                    
                cr.RuleFor(my => my.Month)
                    .GreaterThanOrEqualTo(1)
                    .LessThanOrEqualTo(today.Month)
                    .When(dto => dto.Year == today.Year)
                    .WithName(DisplayNames.Month);
                    
                cr.RuleFor(my => my.Month)
                    .GreaterThanOrEqualTo(1)
                    .LessThanOrEqualTo(12)
                    .When(dto => dto.Year < today.Year)
                    .WithName(DisplayNames.Month);
                #nullable enable
            })
            .When(dto => dto.StatsMonthYear is not null);
    }
    #endregion
}