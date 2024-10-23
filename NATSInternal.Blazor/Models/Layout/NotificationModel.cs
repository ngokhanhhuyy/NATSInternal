

namespace NATSInternal.Blazor.Models;

public class NotificationModel
{
    public int Id { get; set; }
    public string Content { get; set; }
    public string EmittedDeltaText { get; set; }
    public bool IsRead { get; set; }

    public NotificationModel(NotificationResponseDto responseDto)
    {
        Id = responseDto.Id;
        Content = $"Nội dung thông báo id {Id}";
        EmittedDeltaText = DateTime.UtcNow
            .ToApplicationTime()
            .DeltaTextFromDateTime(responseDto.DateTime);
        IsRead = responseDto.IsRead;
    }
}
