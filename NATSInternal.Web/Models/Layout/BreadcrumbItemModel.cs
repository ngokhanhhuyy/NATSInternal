namespace NATSInternal.Web.Models;

public class BreadcrumbItemModel
{
    #region Constructors
    public BreadcrumbItemModel(string name, string? url = null)
    {
        Name = name;
        Url = url;
    }
    #endregion
    
    #region Properties
    public string Name { get; init; }
    public string? Url { get; init; }
    #endregion
}