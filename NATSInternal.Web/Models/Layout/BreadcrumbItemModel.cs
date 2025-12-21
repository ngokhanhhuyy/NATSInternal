namespace NATSInternal.Web.Models;

public class BreadcrumbItemModel
{
    #region Properties
    public required string Name { get; init; }
    public string? Url { get; init; }
    #endregion
}