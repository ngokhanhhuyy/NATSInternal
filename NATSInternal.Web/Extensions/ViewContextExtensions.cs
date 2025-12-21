using Microsoft.AspNetCore.Mvc.Razor;

namespace NATSInternal.Api.Extensions;

public static class RazorPageBaseExtensions
{
    #region ExtensionMethods
    public static string? GetCurrentControllerName(this RazorPageBase page)
    {
        return page.ViewContext.RouteData.Values["controller"]?.ToString();
    }
    
    public static string? GetCurrentActionName(this RazorPageBase page)
    {
        return page.ViewContext.RouteData.Values["action"]?.ToString();
    }

    public static bool IsCurrentControllerName(this RazorPageBase page, string nameToCheck)
    {
        if (page.GetCurrentControllerName() is null)
        {
            return false;
        }
        
        if (nameToCheck.EndsWith("Controller"))
        {
            return page.GetCurrentControllerName() + "Controller" == nameToCheck;
        }
        
        return page.GetCurrentControllerName() == nameToCheck;
    }

    public static bool IsCurrentActionName(this RazorPageBase page, string nameToCheck)
    {
        if (page.GetCurrentActionName() is null)
        {
            return false;
        }
        
        return page.GetCurrentActionName() == nameToCheck;
    }
    #endregion
}