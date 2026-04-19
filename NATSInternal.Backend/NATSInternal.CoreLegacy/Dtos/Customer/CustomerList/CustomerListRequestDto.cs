namespace NATSInternal.Core.Dtos;

public class CustomerListRequestDto : ICreatorTrackableListRequestDto
{
    #region Properties
    public bool? SortByAscending { get; set; }
    public string? SortByFieldName { get; set; }
    public string? SearchContent { get; set; }
    public int? CreatedUserId { get; set; }
    public int? Page { get; set; }
    public int? ResultsPerPage { get; set; }
    public bool? HasRemainingDebtAmountOnly { get; set; }
    #endregion

    #region Methods
    public void TransformValues()
    {
        SearchContent = SearchContent?.ToNullIfEmptyOrWhiteSpace();
    }
    #endregion

    #region Enum
    public enum FieldToSort
    {
        FirstName,
        Birthday,
        CreatedDateTime,
        DebtRemainingAmount,
        LastName
    }
    #endregion
}