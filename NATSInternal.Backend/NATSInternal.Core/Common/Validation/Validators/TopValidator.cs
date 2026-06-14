using FluentValidation;
using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Localization;

namespace NATSInternal.Core.Common.Validation;

public class TopValidator : Validator<TopRequestDto>
{
    #region Constructors
    public TopValidator(TopAndCountValidator<TopRequestDto> topAndCountValidator)
    {
        RuleFor(dto => dto.ResultsCount)
            .NotEmpty()
            .GreaterThanOrEqualTo(3)
            .LessThanOrEqualTo(50)
            .WithName(DisplayNames.ResultsCount);
        Include(topAndCountValidator);
    }
    #endregion
}
