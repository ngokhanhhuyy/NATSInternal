// using FluentValidation;
// using FluentValidation.Results;

// namespace NATSInternal.Core.Common.Extensions;

// public static class ValidationExceptionExtensions
// {
//     #region Extensions
//     extension(ValidationException exception)
//     {
//         public ValidationException AddPropertyPathPrefixElements(IEnumerable<object> prefixPropertyPathElements)
//         {
//             List<ValidationFailure> failures = exception.Errors.Select(existingFailure => new ValidationFailure
//             {
//                 PropertyName = ex
//                 ErrorCode = existingFailure.ErrorCode,
//                 ErrorMessage = existingFailure.ErrorMessage
//             })
//             .ToList();
//         }
//     }
//     #endregion
// }