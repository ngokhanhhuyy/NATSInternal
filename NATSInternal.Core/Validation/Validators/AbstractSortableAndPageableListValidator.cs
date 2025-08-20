namespace NATSInternal.Core.Validation.Validators;

internal abstract class AbstractSortableAndPageableListValidator<TListRequestDto> : Validator<TListRequestDto>
    where TListRequestDto : ISortableAndPagableListRequestDto
{
    #region Constructors
    protected AbstractSortableAndPageableListValidator()
    {
        Include(new AbstractSort)
    }
    #endregion
}