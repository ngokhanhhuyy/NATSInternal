namespace NATSInternal.Desktop.ViewModels;

public class ErrorDetail
{
    #region Constructors
    public ErrorDetail(string propertyPath, string message)
    {
        PropertyPath = propertyPath;
        Message = message;
    }

    public ErrorDetail(object[] propertyPathElements, string message)
    {
        PropertyPath = ConvertPropertyPathElementsToPropertyPath(propertyPathElements);
        Message = message;
    }
    #endregion
    
    #region Properties
    public string PropertyPath { get; }
    public string Message { get; }
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