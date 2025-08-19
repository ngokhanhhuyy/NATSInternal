namespace NATSInternal.Core.Dtos;

public class ProductCategoryListRequestDto : ISortableListRequestDto
{
    public bool? SortingByAscending { get; set; }
    public string SortingByFieldName { get; set; }
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;

    public void TransformValues()
    {
        SortingByFieldName = SortingByFieldName?.ToNullIfEmpty();
    }
}
