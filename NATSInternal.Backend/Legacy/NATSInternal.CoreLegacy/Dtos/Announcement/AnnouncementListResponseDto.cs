namespace NATSInternal.Core.Dtos;

public class AnnouncementListResponseDto : IPageableListResponseDto<AnnouncementResponseDto>
{
    #region Properties
    public List<AnnouncementResponseDto> Items { get; set; } = new();
    public int PageCount { get; set; }
    #endregion
    
    #region Constructors
    internal AnnouncementListResponseDto(ICollection<AnnouncementResponseDto> announcementResponseDtos, int pageCount)
    {
        Items.AddRange(announcementResponseDtos);
        PageCount = pageCount;
    }
    #endregion
}