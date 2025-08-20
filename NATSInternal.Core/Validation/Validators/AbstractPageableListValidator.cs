namespace NATSInternal.Core.Validation.Validators;

internal abstract class AbstractPageableListValidator<TListRequestDto, TFieldToSort> : Validator<TListRequestDto>
        where TListRequestDto : IPageableListRequestDto
        where TFieldToSort : struct, Enum
{
    #region Constructors
    protected AbstractPageableListValidator(
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