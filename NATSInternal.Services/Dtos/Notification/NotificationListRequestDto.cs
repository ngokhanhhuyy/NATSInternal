namespace NATSInternal.Services.Dtos;

public class NotificationListRequestDto : IOrderableListRequestDto
{
    public bool? SortingByAscending { get; set; }
    public string SortingByField { get; set; }
    public bool UnreadOnly { get; set; } = true;
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 5;
    
    public void TransformValues()
    {
    }
}