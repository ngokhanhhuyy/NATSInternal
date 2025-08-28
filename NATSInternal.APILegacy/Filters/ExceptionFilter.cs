using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using FluentValidation.Results;

namespace NATSInternal.API.Filters;

public class ExceptionFilter
{
    #region Fields
    private readonly ProblemDetailsFactory _problemDetailsFactory;
    private readonly JsonNamingPolicy _jsonNamingPolicy;
    #endregion
    
    #region Constructors
    public ExceptionFilter(
        ProblemDetailsFactory problemDetailsFactory,
        IOptions<JsonOptions> jsonOptions)
    {
        _problemDetailsFactory = problemDetailsFactory;
        _jsonNamingPolicy = jsonOptions.Value.JsonSerializerOptions.PropertyNamingPolicy ?? JsonNamingPolicy.CamelCase;
    }
    #endregion

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
                    new Dictionary<string, string>
                    {
                        {
                            PropertyPathElementsToPath(operationException.PropertyPathElements),
                            operationException.Message
                        }
                    }
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
    
    #region StaticMethods
    private string PropertyPathElementsToPath(object[] propertyPathElements)
    {
        StringBuilder pathBuilder = new();
        for (int i = 0; i < propertyPathElements.Length; i++)
        {
            if (propertyPathElements[i] is string stringElement)
            {
                if (i > 0)
                {
                    pathBuilder.Append('.');
                }

                pathBuilder.Append(_jsonNamingPolicy.ConvertName(stringElement));
            }
            else
            {
                pathBuilder.Append($"[{propertyPathElements[i]}]");
            }
        }

        return pathBuilder.ToString();
    }
    #endregion
}