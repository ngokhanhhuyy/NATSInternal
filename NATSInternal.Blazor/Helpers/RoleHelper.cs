namespace NATSInternal.Blazor.Helpers;

public class RoleHelper
{
    public string GetRoleBootstrapColor(string roleName)
    {
        Dictionary<string, string> roleColors = new Dictionary<string, string>
        {
            { "Developer", "danger" },
            { "Manager", "primary" },
            { "Accountant", "success" },
            { "Staff", "secondary" }
        };

        return roleColors[roleName];
    }

    public string GetRoleBootstrapIcon(string roleName)
    {
        Dictionary<string, string> roleIcons = new Dictionary<string, string>
        {
            { "Developer", "bi bi-wrench" },
            { "Manager", "bi bi-star-fill" },
            { "Accountant", "bi bi-star-half" },
            { "Staff", "bi bi-star" }
        };

        return roleIcons[roleName];
    }
}