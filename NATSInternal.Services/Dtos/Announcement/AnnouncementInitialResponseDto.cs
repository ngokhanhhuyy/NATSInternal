namespace NATSInternal.Services.Dtos;

public class AnnouncementInitialResponseDto : IUpsertableInitialResponseDto
{
    public string DisplayName { get; } = DisplayNames.Announcement;
    public required bool CreatingPermission { get; init; }
}