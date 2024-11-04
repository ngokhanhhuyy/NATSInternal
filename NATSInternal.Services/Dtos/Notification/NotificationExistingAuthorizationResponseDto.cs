namespace NATSInternal.Services.Dtos;

internal class NotificationExistingAuthorizationResponseDto
    : IUpsertableExistingAuthorizationResponseDto
{
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
}
