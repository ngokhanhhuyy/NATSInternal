using NATSInternal.Application.Security;
using NATSInternal.Application.UseCases.Users;

namespace NATSInternal.Desktop.Sessions;

public class CurrentUserSession : ICallerDetailProvider
{
    #region Fields
    private UserGetDetailResponseDto? _caller;
    #endregion
    
    #region Properties
    private UserGetDetailResponseDto Caller => _caller
        ?? throw new InvalidOperationException("Caller information is not loaded yet.");
    #endregion
    
    #region Methods
    public Guid GetId() => Caller.Id;
    public string GetUserName() => Caller.UserName;
    public ICollection<string> GetRoleNames() => Caller.Roles.Select(r => r.Name).ToList();
    public ICollection<string> GetPermissionNames() => Caller.Roles.SelectMany(r => r.PermissionNames).ToList();
    public int GetPowerLevel() => Caller.Roles.Max(r => r.PowerLevel);
    public void SetCaller(UserGetDetailResponseDto responseDto) => _caller = responseDto;
    #endregion
}