namespace NATSInternal.Core.Dtos;

public class UserBasicAuthorizationResponseDto : IUpsertableExistingAuthorizationResponseDto
{
    public bool CanEdit { get; set; }
    public bool CanChangePassword { get; set; }
    public bool CanResetPassword { get; set; }
    public bool CanDelete { get; set; }
}
