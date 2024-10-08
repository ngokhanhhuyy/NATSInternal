namespace NATSInternal.Services.Dtos;

public class UserBasicAuthorizationResponseDto : IUpsertableAuthorizationResponseDto
{
    public bool CanEdit { get; set; }
    public bool CanChangePassword { get; set; }
    public bool CanResetPassword { get; set; }
    public bool CanDelete { get; set; }
}
