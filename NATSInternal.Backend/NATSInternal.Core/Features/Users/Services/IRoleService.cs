namespace NATSInternal.Core.Features.Users;

public interface IRoleService
{
    #region Methods
    Task<List<RoleBasicResponseDto>> GetAllAsync();
    Task<RoleDetailResponseDto> GetDetailAsync(int id);
    #endregion
}