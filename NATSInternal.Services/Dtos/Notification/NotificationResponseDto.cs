namespace NATSInternal.Services.Dtos;

public class NotificationResponseDto
{
    public int Id { get; set; }
    public NotificationType Type { get; set; }
    public DateTime DateTime { get; set; }
    public List<int> ResourceIds { get; set; }
    public UserBasicResponseDto CreatedUser { get; set; }
    public bool IsRead { get; set; }
    
    internal NotificationResponseDto(Notification notification, int currentUserId)
    {
        Id = notification.Id;
        Type = notification.Type;
        DateTime = notification.CreatedDateTime;
        ResourceIds = notification.ResourceIds;
        IsRead = notification.ReadUsers.Select(u => u.Id).Contains(currentUserId);
        
        if (notification.CreatedUser != null)
        {
            CreatedUser = new UserBasicResponseDto(notification.CreatedUser);
        }
    }
}