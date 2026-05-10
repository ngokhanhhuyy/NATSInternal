using FluentValidation;
using NATSInternal.Test.Common;

namespace NATSInternal.Test.Extensions;

public static class AssertExtensions
{
    #region ExtensionMethods
    extension(Assert)
    {
        public static void IsValidationExceptionAndErrorsContainFieldName<TResponseDto>(
            ExecutionResult<TResponseDto> result,
            string fieldName)
        {
            Assert.IsType<ValidationException>(result.Exception);

            IEnumerable<string> propertyNames = ((ValidationException)result.Exception!)
                .Errors
                .Select(f => f.PropertyName);
                
            Assert.Contains(fieldName, propertyNames);
        }

        public static void IsValidationExceptionAndErrorsContainOneOfFieldNames<TResponseDto>(
            ExecutionResult<TResponseDto> result,
            params string[] fieldNames)
        {
            Assert.IsType<ValidationException>(result.Exception);

            IEnumerable<string> propertyNames = ((ValidationException)result.Exception!)
                .Errors
                .Select(f => f.PropertyName);
                
            Assert.Contains(propertyNames, name => fieldNames.Contains(name));
        }
    }
    #endregion
}