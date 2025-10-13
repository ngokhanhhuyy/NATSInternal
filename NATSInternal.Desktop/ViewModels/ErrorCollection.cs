// using System.ComponentModel;
// using Avalonia.Collections;
// using FluentValidation;
// using FluentValidation.Results;
// using NATSInternal.Application.Exceptions;
//
// namespace NATSInternal.Desktop.ViewModels;
//
// public class ErrorCollection : AvaloniaDictionary<string, string>
// {
//     #region Fields
//     private bool _hasErrors;
//     #endregion
//     
//     #region Constructors
//     public ErrorCollection()
//     {
//     }
//     #endregion
//     
//     #region Properties
//     public bool HasErrors => Count > 0;
//
//     public IEnumerable<string> Messages => Values;
//     #endregion
//     
//     #region Methods
//     public void AddFromValidationException(ValidationException exception)
//     {
//         foreach (ValidationFailure failure in exception.Errors)
//         {
//             Add(failure.PropertyName, failure.ErrorMessage);
//         }
//     }
//
//     public void AddFromOperationException(OperationException exception)
//     {
//         foreach (KeyValuePair<object[], string> pair in exception.Errors)
//         {
//             string path = ConvertPropertyPathElementsToPropertyPath(pair.Key);
//             Add(path, pair.Value);
//         }
//     }
//
//     public int IndexOf(KeyValuePair<string, string> item)
//     {
//         return Keys.ToList().IndexOf(item.Key);
//     }
//
//     public void Insert(int index, KeyValuePair<string, string> item)
//     {
//         
//     }
//     #endregion
//     
//     #region StaticMethods
//     private static string ConvertPropertyPathElementsToPropertyPath(object[] propertyPathElements)
//     {
//         StringBuilder pathBuilder = new();
//         for (int i = 0; i < propertyPathElements.Length; i++)
//         {
//             object element = propertyPathElements[i];
//             if (element is int intElement)
//             {
//                 pathBuilder.Append($"[{intElement}]");
//                 continue;
//             }
//
//             if (i > 0)
//             {
//                 pathBuilder.Append('.');
//             }
//
//             pathBuilder.Append(element);
//         }
//
//         return pathBuilder.ToString();
//     }
//     #endregion
// }