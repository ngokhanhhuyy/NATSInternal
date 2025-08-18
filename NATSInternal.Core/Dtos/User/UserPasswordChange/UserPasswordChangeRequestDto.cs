namespace NATSInternal.Core.Dtos;

public class UserPasswordChangeRequestDto : IRequestDto
{
    #region Properties
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string ConfirmationPassword { get; set; } = string.Empty;
    #endregion
}
