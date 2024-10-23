namespace NATSInternal.Blazor.Models;

public class NotificationListModel
{
    public List<NotificationModel> Items { get; set; }

    public NotificationListModel(NotificationListResponseDto responseDto)
    {
        Items = responseDto.Items
            .Select(n => new NotificationModel(n))
            .ToList()
            ?? new List<NotificationModel>();
    }
}
