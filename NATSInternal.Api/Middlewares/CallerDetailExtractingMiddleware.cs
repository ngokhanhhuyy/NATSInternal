using NATSInternal.Api.Providers;

namespace NATSInternal.Api.Middlewares;

public class CallerDetailExtractingMiddleware
{
    #region Fields
    private readonly RequestDelegate _next;
    private readonly CallerDetailProvider _callerDetailProvider;
    #endregion

    #region Constructors
    public CallerDetailExtractingMiddleware(RequestDelegate next, CallerDetailProvider callerDetailProvider)
    {
        _next = next;
        _callerDetailProvider = callerDetailProvider;
    }
    #endregion

    #region Methods
    public async Task InvokeAsync(HttpContext context)
    {
        _callerDetailProvider.SetCallerDetail(context.User);
        await _next(context);
    }
    #endregion
}
