namespace NATSInternal.Extensions;

public static class HtmlHelperExtensions
{
    public static string GetDefaultAvatar(this IHtmlHelper htmlHelper, string fullName)
    {
        string replacedFullName = fullName.Replace(" ", "+");
        return "https://ui-avatars.com/api/?name=" +
                $"{replacedFullName}&background=random&size=256";
    }
}