namespace NATSInternal.Services.Dtos;

public class NotificationListRequestDto : IOrderableListRequestDto
{
    public bool OrderByAscending { get; set; }
    public string OrderByField { get; set; }
    public bool UnreadOnly { get; set; } = true;
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 5;
    
    public void TransformValues()
    {
    }
}