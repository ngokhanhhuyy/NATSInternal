namespace NATSInternal.Core.Features.Users;

public interface IUserService
{
    #region Methods
    Task<UserListResponseDto> GetListAsync(UserListRequestDto requestDto);
    Task<UserDetailResponseDto> GetDetailByIdAsync(int id);
    Task<UserDetailResponseDto> GetDetailByUserNameAsync(string userName, bool includingAuthorization = false);
    Task<int> CreateAsync(UserCreateRequestDto requestDto);
    Task UpdateAsync(int id, UserUpdateRequestDto requestDto);
    Task DeleteAsync(int id);
    Task RestoreAsync(int id);
    #endregion
}