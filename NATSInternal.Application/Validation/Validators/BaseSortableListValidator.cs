using FluentValidation;
using NATSInternal.Application.Localization;
using NATSInternal.Application.UseCases;
using NATSInternal.Application.Validation.Rules;

namespace NATSInternal.Application.Validation.Validators;

internal class BaseSortableListValidator<TListRequestDto, TFieldToSort> : Validator<TListRequestDto>
        where TListRequestDto : ISortableListRequestDto
        where TFieldToSort : struct, Enum
{
    #region Constructors
    public BaseSortableListValidator()
    {
        RuleFor(dto => dto.SortByFieldName)
            .IsOneOfFieldsToSort<TListRequestDto, TFieldToSort>()
            .WithName(DisplayNames.SortByFieldName);
    }
    #endregion
}