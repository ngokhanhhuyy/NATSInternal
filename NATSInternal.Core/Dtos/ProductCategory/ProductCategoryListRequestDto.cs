namespace NATSInternal.Core.Dtos;

public class ProductCategoryListRequestDto : ISortableAndPageableListRequestDto
{
    #region Properties
    public bool? SortingByAscending { get; set; }
    public string? SortingByFieldName { get; set; }
    public int? Page { get; set; }
    public int? ResultsPerPage { get; set; }
    #endregion

    #region Methods
    public void TransformValues()
    {
        SortingByFieldName = SortingByFieldName?.ToNullIfEmptyOrWhiteSpace();
    }
    #endregion

    #region Enum
    public enum FieldToSort
    {
        CreatedDateTime
    }
    #endregion
}