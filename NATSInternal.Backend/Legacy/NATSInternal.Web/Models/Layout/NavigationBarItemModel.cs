namespace NATSInternal.Web.Models;

public class NavigationBarItemModel
{
    #region Constructors
    public NavigationBarItemModel(string controller, string action, string iconClassName)
    {
        Controller = controller.Replace("Controller", "");
        Action = action;
        IconClassName = iconClassName;
    }
    #endregion

    #region Properties
    public string Controller { get; }
    public string Action { get; }
    public string IconClassName { get; }
    #endregion
}