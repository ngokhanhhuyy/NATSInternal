namespace NATSInternal.Services.Dtos;

public class NotificationListRequestDto : IRequestDto
{
    public bool UnreadOnly { get; set; } = true;
    public int Page { get; set; } = 1;
    public int ResultsPerPage { get; set; } = 5;
    
    public void TransformValues()
    {
    }
}