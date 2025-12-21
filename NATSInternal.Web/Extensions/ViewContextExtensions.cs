using Microsoft.AspNetCore.Mvc.Rendering;

namespace NATSInternal.Web.Extensions;

public static class ViewContextExtensions
{
    #region ExtensionMethods
    public static string? GetCurrentControllerName(this ViewContext viewContext)
    {
        return viewContext.RouteData.Values["controller"]?.ToString();
    }
    
    public static string? GetCurrentActionName(this ViewContext viewContext)
    {
        return viewContext.RouteData.Values["action"]?.ToString();
    }

    public static bool IsCurrentControllerName(this ViewContext viewContext, string nameToCheck)
    {
        if (viewContext.GetCurrentControllerName() is null)
        {
            return false;
        }
        
        if (nameToCheck.EndsWith("Controller"))
        {
            return viewContext.GetCurrentControllerName() + "Controller" == nameToCheck;
        }
        
        return viewContext.GetCurrentControllerName() == nameToCheck;
    }

    public static bool IsCurrentActionName(this ViewContext viewContext, string nameToCheck)
    {
        if (viewContext.GetCurrentActionName() is null)
        {
            return false;
        }
        
        return viewContext.GetCurrentActionName() == nameToCheck;
    }
    #endregion
}