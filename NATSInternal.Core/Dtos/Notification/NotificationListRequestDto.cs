namespace NATSInternal.Core.Dtos;

public class NotificationListRequestDto : ISortableListRequestDto
{
    public bool? SortingByAscending { get; set; }
    public string SortingByFieldName { get; set; }
    public bool UnreadOnly { get; set; } = true;
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 5;
    
    public void TransformValues()
    {
    }
}