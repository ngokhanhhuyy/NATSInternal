using FluentValidation;
using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Localization;

namespace NATSInternal.Core.Common.Validation;

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
    }
    #endregion
}