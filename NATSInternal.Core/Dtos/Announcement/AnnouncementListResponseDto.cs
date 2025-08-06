namespace NATSInternal.Core.Dtos;

public class AnnouncementListResponseDto
{
    public List<AnnouncementResponseDto> Items { get; set; }
    public int PageCount { get; set; }
}