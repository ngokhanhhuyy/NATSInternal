using NATSInternal.Api.Providers;

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
        CallerDetailProvider callerDetailProvider = context.RequestServices.GetRequiredService<CallerDetailProvider>();
        callerDetailProvider.SetCallerDetail(context.User);
        await _next(context);
    }
    #endregion
}