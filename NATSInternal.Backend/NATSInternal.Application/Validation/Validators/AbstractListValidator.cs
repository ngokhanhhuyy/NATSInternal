using FluentValidation;
using NATSInternal.Application.Localization;
using NATSInternal.Application.UseCases;
using NATSInternal.Application.Validation.Rules;

namespace NATSInternal.Application.Validation.Validators;

internal abstract class AbstractListValidator<TListRequestDto, TFieldToSort> : Validator<TListRequestDto>
        where TListRequestDto : IListRequestDto
        where TFieldToSort : struct, Enum
{
    #region Constructors
    protected AbstractListValidator(
        int pageMinValue = 1,
        int resultsPerPageMinValue = 5,
        int resultsPerPageMaxValue = 50)
    {
        RuleFor(dto => dto.SortByFieldName)
            .IsOneOfFieldsToSort<TListRequestDto, TFieldToSort>()
            .WithName(DisplayNames.SortByFieldName);
        RuleFor(dto => dto.Page)
            .GreaterThanOrEqualTo(pageMinValue)
            .WithName(DisplayNames.Page);
        RuleFor(dto => dto.ResultsPerPage)
            .GreaterThanOrEqualTo(resultsPerPageMinValue)
            .LessThanOrEqualTo(resultsPerPageMaxValue)
            .WithName(DisplayNames.ResultsPerPage);
        RuleFor(dto => dto.SearchContent)
            .MaximumLength(255)
            .WithName(DisplayNames.SearchContent);
    }
    #endregion
}