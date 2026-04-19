namespace NATSInternal.Core.Exceptions;

public abstract class CoreException : Exception
{
    #region Constructors
    protected CoreException() { }


    protected CoreException(string message) : this(Array.Empty<object>(), message) { }
    
    protected CoreException(object[] propertyPathElements, string message)
    {
        Errors.Add(propertyPathElements, message);
    }
    #endregion
    
    #region Properties
    public Dictionary<object[], string> Errors { get; } = new();
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