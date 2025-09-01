using FluentValidation;
using NATSInternal.Application.Localization;
using NATSInternal.Application.UseCases;

namespace NATSInternal.Application.Validation.Validators;

internal class BasePageableListValidator<TListRequestDto> : Validator<TListRequestDto>
        where TListRequestDto : IPageableListRequestDto
{
    #region Constructors
    public BasePageableListValidator(
        int pageMinValue = 1,
        int resultsPerPageMinValue = 5,
        int resultsPerPageMaxValue = 50)
    {
        RuleFor(dto => dto.Page)
            .GreaterThanOrEqualTo(pageMinValue)
            .WithName(DisplayNames.Page);
        RuleFor(dto => dto.ResultsPerPage)
            .GreaterThanOrEqualTo(resultsPerPageMinValue)
            .LessThanOrEqualTo(resultsPerPageMaxValue)
            .WithName(DisplayNames.ResultsPerPage);
    }
    #endregion
}