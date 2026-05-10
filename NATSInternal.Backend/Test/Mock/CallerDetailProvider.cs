using NATSInternal.Core.Common.Security;
using NATSInternal.Core.Features.Users;
using NATSInternal.Test.Mock;

namespace NATSInternal.Test.Mock;

public class CallerDetailProvider : ICallerDetailProvider
{
    #region Fields
    private int _id;
    private string _userName = string.Empty;
    private readonly List<string> _roleNames = new();
    private readonly List<string> _permissionNames = new();
    private int _powerLevel;
    #endregion

    #region Methods
    public async Task InitializeByRoleNameAsync(CallerDetailContext callerContext, string roleName)
    {
        UserDetailResponseDto detailResponseDto = await callerContext.GetCallerDetailByRoleNameAsync(roleName);
        _id = detailResponseDto.Id;
        _userName = detailResponseDto.UserName;
        _roleNames.AddRange(detailResponseDto.Roles.Select(r => r.Name));
        _permissionNames.AddRange(detailResponseDto.Roles.SelectMany(r => r.PermissionNames));
        _powerLevel = detailResponseDto.Roles.Max(r => r.PowerLevel);
    }

    public int GetId() => _id;
    public string GetUserName() => _userName;
    public ICollection<string> GetRoleNames() => _roleNames;
    public ICollection<string> GetPermissionNames() => _permissionNames;
    public int GetPowerLevel() => _powerLevel;
    #endregion
}