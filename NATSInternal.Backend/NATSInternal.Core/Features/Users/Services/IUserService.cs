namespace NATSInternal.Core.Features.Users;

public interface IUserService
{
    #region Methods
    Task<UserListResponseDto> GetListAsync(UserListRequestDto requestDto);
    Task<UserDetailResponseDto> GetDetailAsync(int id);
    Task<int> CreateAsync(UserCreateRequestDto requestDto);
    Task UpdateAsync(UserUpdateRequestDto requestDto);
    Task ChangePasswordAsync(UserChangePasswordRequestDto requestDto);
    Task ResetPasswordAsync(UserResetPasswordRequestDto requestDto);
    Task DeleteAsync(int id);
    Task RestoreAsync(int id);
    Task VerifyUserNameAndPasswordAsync(UserVerifyUserNameAndPasswordRequestDto requestDto);
    #endregion
}