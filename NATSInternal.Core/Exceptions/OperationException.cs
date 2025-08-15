namespace NATSInternal.Core.Exceptions;

public class OperationException : CoreException
{
    #region Constructors
    public OperationException(string message) : base(message) { }

    public OperationException(object[] propertyPathElements, string message) : base(propertyPathElements, message) { }
    #endregion

    #region Methods
    public static OperationException NotFound(object[] propertyPathElements, string resourceDisplayName)
    {
        string errorMessage = ErrorMessages.NotFound.ReplaceResourceName(resourceDisplayName);
        return new(propertyPathElements, errorMessage);
    }

    public static OperationException Duplicated(object[] propertyPathElements, string displayName)
    {
        string message = ErrorMessages.Duplicated.Replace("{ResourceName}", displayName);
        return new(propertyPathElements, message);
    }

    public static OperationException DeleteRestricted(object[] propertyPathElements, string resourceDisplayName)
    {
        string message = ErrorMessages.DeleteRestricted.Replace("{ResourceName}", resourceDisplayName);
        return new(propertyPathElements, message);
    }
    #endregion
}
