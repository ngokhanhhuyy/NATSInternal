namespace NATSInternal.Core.Validation.Validators;

internal abstract class AbstractSortableListValidator<TListRequestDto, TFieldToSort> : Validator<TListRequestDto>
        where TListRequestDto : ISortableListRequestDto
        where TFieldToSort : struct, Enum
{
    #region Constructors
    protected AbstractSortableListValidator()
    {
        RuleFor(dto => dto.SortingByFieldName)
            .IsOneOfFieldsToSort<TListRequestDto, TFieldToSort>()
            .WithName(DisplayNames.SortByFieldName);
    }
    #endregion
}