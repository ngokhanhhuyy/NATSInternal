using System.Security.Authentication;
using System.Security.Claims;
using NATSInternal.Application.Security;

namespace NATSInternal.Web.Providers;

public class CallerDetailProvider : ICallerDetailProvider
{
    #region Fields
    private Guid? _id;
    private string? _userName;
    private readonly List<string> _roleNames = new();
    private readonly List<string> _permissionNames = new();
    private int _powerLevel;
    #endregion
    
    #region Methods
    public void SetCallerDetail(ClaimsPrincipal principal)
    {
        try
        {
            // Extract id.
            string idAsString = principal.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new AuthenticationException();
            _id = Guid.Parse(idAsString);

            // Extract userName.
            _userName = principal.FindFirstValue(ClaimTypes.Name) ?? throw new AuthenticationException();

            // Extract role names.
            _roleNames.AddRange(principal.FindAll(ClaimTypes.Role).Select(c => c.Value));

            // Extract permission names.
            _permissionNames.AddRange(principal.FindAll("Permission").Select(c => c.Value));
            
            // Extract power level.
            string powerLevelAsString = principal.FindFirstValue("PowerLevel") ?? throw new AuthenticationException();
            _powerLevel = int.Parse(powerLevelAsString);
        }
        catch (FormatException)
        {
            throw new AuthenticationException();
        }
    }

    public Guid GetId()
    {
        return _id ?? throw GenerateException();
    }

    public string GetUserName()
    {
        return _userName ?? throw GenerateException();
    }

    public ICollection<string> GetRoleNames()
    {
        return _roleNames ?? throw GenerateException();
    }

    public ICollection<string> GetPermissionNames()
    {
        return _permissionNames ?? throw GenerateException();
    }

    public int GetPowerLevel()
    {
        return _powerLevel;
    }
    #endregion
    
    #region StaticMethods
    private static InvalidOperationException GenerateException()
    {
        return new("Caller detail has not been loaded yet.");
    }
    #endregion
        
}