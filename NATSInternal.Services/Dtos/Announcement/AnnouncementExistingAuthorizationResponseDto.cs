namespace NATSInternal.Services.Dtos;

public class AnnouncementExistingAuthorizationResponseDto
        : IUpsertableExistingAuthorizationResponseDto
{
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
}
