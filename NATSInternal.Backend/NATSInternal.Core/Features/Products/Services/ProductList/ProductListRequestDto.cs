using JetBrains.Annotations;
using NATSInternal.Core.Common.Dtos;
using NATSInternal.Core.Common.Extensions;

namespace NATSInternal.Core.Features.Products;

[UsedImplicitly]
public class ProductListRequestDto : ISearchableListRequestDto
{
    #region Properties
    public bool SortByAscending { get; set; } = true;
    public string SortByFieldName { get; set; } = nameof(FieldToSort.Status);
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;
    public List<int> CategoryIds { get; set; } = new();
    public string? SearchContent { get; set; }
    #endregion

    #region Methods
    public void TransformValues()
    {
        SearchContent = SearchContent.ToNullIfEmptyOrWhiteSpace();
    }
    #endregion

    #region Enums
    public enum FieldToSort
    {
        Status,
        Name,
        DefaultAmountBeforeVatPerUnit,
        CreatedDateTime,
        StockingQuantity
    }
    #endregion
}