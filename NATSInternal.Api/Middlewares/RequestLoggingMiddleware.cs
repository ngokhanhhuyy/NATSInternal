using System.Globalization;

namespace NATSInternal.Api.Middlewares;

public class RequestLoggingMiddleware
{
    #region Fields
    private readonly RequestDelegate _next;
    #endregion

    #region Constructors
    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    #endregion

    #region Methods
    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);
        string method = context.Request.Method;
        string path = context.Request.Path;
        QueryString queryString = context.Request.QueryString;
        int statusCode = context.Response.StatusCode;
        string statusColor = statusCode switch
        {
            >= 200 and < 300 => "\e[42m\e[37m", // Green
            >= 300 and < 400 => "\e[43m\e[30m", // Yellow
            >= 400 and < 500 => "\e[41m\e[37m", // Red
            >= 500 => "\e[46m\e[30m",           // Cyan
            _ => ""
        };
        string currentDateTimeAsString = DateTime.Now.ToString(CultureInfo.InvariantCulture);

        string logEntry =
            $"{statusColor}{statusCode}\e[0m     " +
            $"\e[47m\e[30m{currentDateTimeAsString}\e[0m " +
            $"{method} {path}{queryString}";

        Console.WriteLine(logEntry);
    }
    #endregion
}