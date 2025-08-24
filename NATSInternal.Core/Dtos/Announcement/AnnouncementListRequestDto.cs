namespace NATSInternal.Core.Dtos;

public class AnnouncementListRequestDto : ISortableListRequestDto
{
    public bool? SortByAscending { get; set; }
    public string SortByFieldName { get; set; }
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;

    public void TransformValues()
    {
    }
}