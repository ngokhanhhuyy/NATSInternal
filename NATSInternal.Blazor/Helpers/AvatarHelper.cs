namespace NATSInternal.Blazor.Helpers;

public class AvatarHelper
{
    public string GetDefaultAvatar(string fullName)
    {
        return $"https://ui-avatars.com/api/?name={fullName.Replace(" ", "+")}" +
            "&background=random&size=256";
    }
}
