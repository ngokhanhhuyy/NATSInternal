using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Extensions;

namespace NATSInternal.Core.Features.Customers;

public class CustomerListRequestDto : ISearchableListRequestDto
{
    #region Properties
    public bool SortByAscending { get; set; } = true;
    public string SortByFieldName { get; set; } = nameof(FieldToSort.LastName);
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    public string? SearchContent { get; set; }
    public int? ExcludedId { get; set; } = new();
    #endregion
    
    #region Methods
    public void TransformValues()
    {
        SearchContent = SearchContent.ToNullIfEmptyOrWhiteSpace();
        ExcludedId = ExcludedId == 0 ? null : ExcludedId;
    }
    #endregion
    
    #region Enums
    public enum FieldToSort
    {
        LastName,
        FirstName,
        Birthday,
        CreatedDateTime,
        DebtRemainingAmount
    }
    #endregion
}
