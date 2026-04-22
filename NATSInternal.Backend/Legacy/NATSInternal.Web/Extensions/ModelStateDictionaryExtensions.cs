using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NATSInternal.Application.Exceptions;
using System.Text;

namespace NATSInternal.Web.Extensions;

public static class ModelStateDictionaryExtensions
{
    #region ExtensionMethods
    public static void AddModelErrors(this ModelStateDictionary modelState, ValidationException exception)
    {
        foreach (ValidationFailure failure in exception.Errors)
        {
            modelState.AddModelError(failure.PropertyName, failure.ErrorMessage);
        }
    }
    
    public static void AddModelErrors(this ModelStateDictionary modelState, OperationException exception)
    {
        foreach (KeyValuePair<object[], string> pair in exception.Errors)
        {
            string propertyPath = ConvertPropertyPathElementsToPropertyPath(pair.Key);
            modelState.AddModelError(propertyPath, pair.Value);
        }
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