using FluentValidation;
using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Localization;

namespace NATSInternal.Core.Common.Validation;

public class TopValidator : TopAndCountValidator<TopRequestDto>
{
    #region Constructors
    public TopValidator()
    {
        RuleFor(dto => dto.ResultsCount)
            .NotEmpty()
            .GreaterThanOrEqualTo(3)
            .LessThanOrEqualTo(50)
            .WithName(DisplayNames.ResultsCount);
    }
    #endregion
}
