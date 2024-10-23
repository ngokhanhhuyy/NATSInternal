namespace NATSInternal.Blazor.Helpers;

public static class AvatarHelper
{
    public static string GetDefaultAvatar(string fullName)
    {
        return $"https://ui-avatars.com/api/?name={fullName.Replace(" ", "+")}" +
            "&background=random&size=256";
    }
}
