namespace NATSInternal.Services.Dtos;

public class AnnouncementUpsertRequestDto : IRequestDto
{
    public AnnouncementCategory Category { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime? StartingDateTime { get; set; }
    public int IntervalInMinutes { get; set; }

    public void TransformValues()
    {
        Title = Title?.ToNullIfEmpty();
        Content = Content?.ToNullIfEmpty();
    }
}