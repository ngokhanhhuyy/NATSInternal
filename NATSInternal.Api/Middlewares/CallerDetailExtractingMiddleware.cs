using System.Security.Claims;
using NATSInternal.Application.Exceptions;

namespace NATSInternal.Api.Middlewares;

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
        ClaimsPrincipal userPrinciple = context.User;

    }
    
    private static async Task LoadCallerAsync(HttpContext httpContext, IAuthorizationService service)
    {
        string? idAsString = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (idAsString is null)
        {
            return;
        }

        bool isParsable = Guid.TryParse(idAsString, out Guid id);
        if (!isParsable)
        {
            throw new AuthenticationException();
        }

        bool isLoadedSuccessfully = await service.LoadCallerAsync(id, httpContext.RequestAborted);
        if (!isLoadedSuccessfully)
        {
            throw new AuthenticationException();
        }
    }
    #endregion
}
