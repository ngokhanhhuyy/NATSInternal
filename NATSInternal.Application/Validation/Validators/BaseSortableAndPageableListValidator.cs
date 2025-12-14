using NATSInternal.Application.UseCases;

namespace NATSInternal.Application.Validation.Validators;

internal class BaseSortableAndPageableListValidator<TListRequestDto, TFieldToSort> : Validator<TListRequestDto>
    where TListRequestDto : ISortableListRequestDto, IPageableListRequestDto
    where TFieldToSort : struct, Enum
{
    #region Constructors
    protected BaseSortableAndPageableListValidator(
        int pageMinValue = 1,
        int resultsPerPageMinValue = 5,
        int resultsPerPageMaxValue = 50)
    {
        Include(new BaseSortableListValidator<TListRequestDto, TFieldToSort>());
        Include(new BasePageableListValidator<TListRequestDto>(
            pageMinValue,
            resultsPerPageMinValue,
            resultsPerPageMaxValue
        ));
    }
    #endregion
}