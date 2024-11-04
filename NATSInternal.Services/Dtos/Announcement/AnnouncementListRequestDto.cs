namespace NATSInternal.Services.Dtos;

public class AnnouncementListRequestDto : ISortableListRequestDto
{
    public bool? SortingByAscending { get; set; }
    public string SortingByField { get; set; }
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 15;

    public void TransformValues()
    {
    }
}