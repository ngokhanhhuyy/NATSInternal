using FluentValidation;
using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Localization;

namespace NATSInternal.Core.Common.Validation;

public class TopAndCountValidator<TRequestDto> : Validator<TRequestDto> where TRequestDto : ITopAndCountRequestDto
{
    #region Constructors
    public TopAndCountValidator()
    {
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
