namespace NATSInternal.Services.Dtos;

public class AnnouncementResponseDto
        : IUpsertableBasicResponseDto<AnnouncementExistingAuthorizationResponseDto>
{
    public int Id { get; set; }
    public AnnouncementCategory Category { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime StartingDateTime { get; set; }
    public DateTime EndingDateTime { get; set; }
    public UserBasicResponseDto CreatedUser { get; set; }
    public AnnouncementExistingAuthorizationResponseDto Authorization { get; set; }

    internal AnnouncementResponseDto(
            Announcement announcement,
            AnnouncementExistingAuthorizationResponseDto authorization)
    {
        Id = announcement.Id;
        Category = announcement.Category;
        Title = announcement.Title;
        Content = announcement.Content;
        StartingDateTime = announcement.StartingDateTime;
        EndingDateTime = announcement.EndingDateTime;
        CreatedUser = new UserBasicResponseDto(announcement.CreatedUser);
        Authorization = authorization;
    }
}