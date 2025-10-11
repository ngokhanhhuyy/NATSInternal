namespace NATSInternal.Application.UseCases.Users;

public interface IGetUserDetailResponseDto
{
    #region Properties
    bool IncludingAuthorization { get; set; }
    #endregion
}