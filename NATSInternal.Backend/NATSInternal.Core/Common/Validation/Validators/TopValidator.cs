using FluentValidation;
using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Localization;
using System.Numerics;

namespace NATSInternal.Core.Common.Validation;

public class TopValidator : Validator<TopRequestDto>
{
    #region Constructors
    public TopValidator()
    {
        RuleFor(dto => dto.ResultsCount)
            .NotEmpty()
            .GreaterThanOrEqualTo(3)
            .LessThanOrEqualTo(50)
            .WithName(DisplayNames.ResultsCount);
        RuleFor(dto => dto.TimeRangeUnitType)
            .NotNull()
            .IsInEnum()
            .WithName(DisplayNames.TimeRangeUnitType);
        RuleFor(dto => dto.TimeRangeUnitCount)
            .NotNull()
            .GreaterThanOrEqualTo(1)
            .WithName(DisplayNames.TimeRangeUnitCount);
    }
    #endregion
}
