namespace NATSInternal.Extensions;

public static class HtmlHelperExtensions
{
    public static string DefaultAvatar(this IHtmlHelper htmlHelper, string fullName)
    {
        string replacedFullName = fullName.Replace(" ", "+");
        return "https://ui-avatars.com/api/?name=" +
                $"{replacedFullName}&background=random&size=256";
    }

    public static string RoleBootstrapColor(this IHtmlHelper htmlHelper, string roleName)
    {
        Dictionary<string, string> roleNameColors = new Dictionary<string, string>
        {
            { RoleConstants.Developer, "danger" },
            { RoleConstants.Manager, "primary" },
            { RoleConstants.Accountant, "success" },
            { RoleConstants.Staff, "secondary" },
        };

        return roleNameColors[roleName];
    }

    public static string RoleBootstrapIcon(this IHtmlHelper htmlHelper, string roleName)
    {
        Dictionary<string, string> roleNameIcons = new Dictionary<string, string>
        {
            { RoleConstants.Developer, "bi bi-wrench" },
            { RoleConstants.Manager, "bi bi-star-fill" },
            { RoleConstants.Accountant, "bi bi-star-half" },
            { RoleConstants.Staff, "bi bi-star" },
        };

        return roleNameIcons[roleName];
    }
}