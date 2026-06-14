using FluentValidation;
using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Localization;

namespace NATSInternal.Core.Common.Validation;

public class CountValidator : Validator<CountRequestDto>
{
    #region Constructors
    public CountValidator(TopAndCountValidator<CountRequestDto> topAndCountValidator)
    {
        Include(topAndCountValidator);
    }
    #endregion
}
