namespace NATSInternal.Core.Dtos;

public class ProductListRequestDto : ISortableAndPageableListRequestDto
{
    #region Properties
    public bool? SortByAscending { get; set; }
    public string? SortByFieldName { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? BrandId { get; set; }
    public string? ProductName { get; set; }
    public int? Page { get; set; } = 1;
    public int? ResultsPerPage { get; set; } = 15;
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion

    #region Enums
    public enum FieldToSort
    {
        CreatedDateTime,
        StockingQuantity
    }
    #endregion
}
