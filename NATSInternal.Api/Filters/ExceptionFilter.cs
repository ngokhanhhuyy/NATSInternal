using System.Security.Authentication;
using System.Text;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using NATSInternal.Application.Exceptions;

namespace NATSInternal.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    #region Fields
    private readonly ProblemDetailsFactory _problemDetailsFactory;
    private readonly JsonNamingPolicy _jsonNamingPolicy;
    private readonly string _authenticationCookieName;
    #endregion

    #region Constructors
    public ExceptionFilter(
        ProblemDetailsFactory problemDetailsFactory,
        IOptions<JsonOptions> jsonOptions,
        IOptionsMonitor<CookieAuthenticationOptions> cookieOptionsMonitor)
    {
        _problemDetailsFactory = problemDetailsFactory;
        _jsonNamingPolicy = jsonOptions.Value.JsonSerializerOptions.PropertyNamingPolicy ?? JsonNamingPolicy.CamelCase;

        const string schemeName = CookieAuthenticationDefaults.AuthenticationScheme;
        CookieAuthenticationOptions options = cookieOptionsMonitor.Get(schemeName);
        _authenticationCookieName = options.Cookie.Name!;
    }
    #endregion

    #region Methods
    public void OnException(ExceptionContext context)
    {
        ProblemDetails details;
        if (context.Exception is ValidationException validationException)
        {
            details = _problemDetailsFactory.CreateProblemDetails(
                context.HttpContext,
                statusCode: 400,
                title: "Bad Request"
            );

            Dictionary<string, string> errorsMap = validationException.Errors.ToDictionary(
                failure => failure.PropertyName,
                failure => failure.ErrorMessage);

            details.Extensions = new Dictionary<string, object?>
            {
                { _jsonNamingPolicy.ConvertName("errors"), errorsMap }
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = 400
            };

            context.ExceptionHandled = true;
            return;
        }

        if (context.Exception is System.ComponentModel.DataAnnotations.ValidationException)
        {
            throw new InvalidOperationException();
        }

        if (context.Exception is NotFoundException)
            {
                details = _problemDetailsFactory.CreateProblemDetails(
                    context.HttpContext,
                    statusCode: 404,
                    title: "Not Found"
                );

                context.Result = new ObjectResult(details)
                {
                    StatusCode = 404
                };

                context.ExceptionHandled = true;
                return;
            }

        if (context.Exception is OperationException operationException)
        {

            details = _problemDetailsFactory.CreateProblemDetails(
                context.HttpContext,
                statusCode: 422,
                title: "Unprocessable Entity"
            );

            details.Extensions = new Dictionary<string, object?>
            {
                {
                    _jsonNamingPolicy.ConvertName("errors"),
                    operationException.Errors.ToDictionary(
                        pair => ConvertPropertyPathElementsToPropertyPath(pair.Key),
                        pair => pair.Value
                    )
                }
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = 422
            };

            context.ExceptionHandled = true;
        }

        if (context.Exception is not ConcurrencyException)
        {
            return;
        }

        details = _problemDetailsFactory.CreateProblemDetails(
            context.HttpContext,
            statusCode: 409,
            title: "Conflict"
        );

        context.Result = new ObjectResult(details)
        {
            StatusCode = 409
        };

        context.ExceptionHandled = true;
    }
    #endregion
    
    #region StaticMethods
    private static string ConvertPropertyPathElementsToPropertyPath(object[] propertyPathElements)
    {
        StringBuilder pathBuilder = new();
        for (int i = 0; i < propertyPathElements.Length; i++)
        {
            object element = propertyPathElements[i];
            if (element is int intElement)
            {
                pathBuilder.Append($"[{intElement}]");
                continue;
            }

            if (i > 0)
            {
                pathBuilder.Append('.');
            }

            pathBuilder.Append(element);
        }

        return pathBuilder.ToString();
    }
    #endregion
}