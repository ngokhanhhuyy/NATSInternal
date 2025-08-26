namespace NATSInternal.Core.Dtos;

public class AnnouncementResponseDto : IUpsertableBasicResponseDto<AnnouncementExistingAuthorizationResponseDto>
{
    #region Properties
    public Guid Id { get; internal set; }
    public AnnouncementCategory Category { get; internal set; }
    public string Title { get; internal set; }
    public string Content { get; internal set; }
    public DateTime StartingDateTime { get; internal set; }
    public DateTime EndingDateTime { get; internal set; }
    public UserBasicResponseDto CreatedUser { get; internal set; }
    public AnnouncementExistingAuthorizationResponseDto? Authorization { get; internal set; }
    #endregion

    #region Constructors
    internal AnnouncementResponseDto(Announcement announcement)
    {
        Id = announcement.Id;
        Category = announcement.Category;
        Title = announcement.Title;
        Content = announcement.Content;
        StartingDateTime = announcement.StartingDateTime;
        EndingDateTime = announcement.EndingDateTime;
        CreatedUser = new(announcement.CreatedUser);
    }

    internal AnnouncementResponseDto(
            Announcement announcement,
            AnnouncementExistingAuthorizationResponseDto authorizationResponseDto) : this(announcement)
    {
        Authorization = authorizationResponseDto;
    }
    #endregion
}