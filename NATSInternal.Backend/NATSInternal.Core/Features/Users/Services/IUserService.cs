namespace NATSInternal.Core.Features.Users;

public interface IUserService
{
    #region Methods
    Task<UserListResponseDto> GetListAsync(UserListRequestDto requestDto);
    Task<UserDetailResponseDto> GetDetailAsync(int id);
    Task<int> CreateAsync(UserCreateRequestDto requestDto);
    Task UpdateAsync(UserUpdateRequestDto requestDto);
    Task DeleteAsync(int id);
    Task RestoreAsync(int id);
    #endregion
}