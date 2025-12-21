using System.Security.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using NATSInternal.Web.Providers;

namespace NATSInternal.Web.Middlewares;

public class CallerDetailExtractingMiddleware
{
    #region Fields
    private readonly RequestDelegate _next;
    #endregion

    #region Constructors
    public CallerDetailExtractingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    #endregion

    #region Methods
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity is null || !context.User.Identity.IsAuthenticated)
        {
            await _next(context);
            return;
        }
        
        CallerDetailProvider callerDetailProvider = context.RequestServices.GetRequiredService<CallerDetailProvider>();

        try
        {
            callerDetailProvider.SetCallerDetail(context.User);
            await _next(context);
        }
        catch (AuthenticationException)
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        }
    }
    #endregion
}