namespace NATSInternal.Core.Dtos;

public class BrandListRequestDto : ISortableAndPageableListRequestDto
{
    #region Properties
    public bool? SortByAscending { get; set; }
    public string? SortByFieldName { get; set; }
    public int? Page { get; set; }
    public int? ResultsPerPage { get; set; }
    #endregion

    #region Methods
    public void TransformValues()
    {
        SortByFieldName = SortByFieldName.ToNullIfEmptyOrWhiteSpace();
    }
    #endregion

    #region Enum
    public enum FieldToSort
    {
        CreatedDateTime
    }
    #endregion
}
