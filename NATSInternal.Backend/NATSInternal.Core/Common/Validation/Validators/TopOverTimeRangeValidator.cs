using FluentValidation;
using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Localization;

namespace NATSInternal.Core.Common.Validation;

public class TopOverTimeRangeValidator : TopAndCountOverTimeRangeValidator<TopRequestDto>
{
    #region Constructors
    public TopOverTimeRangeValidator()
    {
        RuleFor(dto => dto.ResultsCount)
            .NotEmpty()
            .GreaterThanOrEqualTo(3)
            .LessThanOrEqualTo(50)
            .WithName(DisplayNames.ResultsCount);
    }
    #endregion
}
