namespace NATSInternal.Core.Dtos;

public class ProductListRequestDto : ISortableListRequestDto
{
    #region Properties
    public bool? SortingByAscending { get; set; }
    public string? SortingByFieldName { get; set; }
    public int? CategoryId { get; set; }
    public int? BrandId { get; set; }
    public string? ProductName { get; set; }
    public int? Page { get; set; } = 1;
    public int? ResultsPerPage { get; set; } = 15;
    #endregion

    #region Methods
    public void TransformValues() { }
    #endregion
}
