using NATSInternal.Core.Common.Extensions;
using NATSInternal.Core.Common.Localization;

namespace NATSInternal.Core.Common.Exceptions;

public class OperationException : ApplicationException
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
        string message = ErrorMessages.Duplicated.Replace("{PropertyName}", displayName);
        return new(propertyPathElements, message);
    }

    public static OperationException DeleteRestricted(object[] propertyPathElements, string resourceDisplayName)
    {
        string message = ErrorMessages.DeleteRestricted.Replace("{ResourceName}", resourceDisplayName);
        return new(propertyPathElements, message);
    }

    public static OperationException DeleteRestricted(string resourceDisplayName)
    {
        string message = ErrorMessages.DeleteRestricted.Replace("{ResourceName}", resourceDisplayName);
        return new(Array.Empty<object>(), message);
    }
    #endregion
}
