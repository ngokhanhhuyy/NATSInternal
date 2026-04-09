using System.Text;

namespace NATSInternal.Application.Exceptions;

public abstract class ApplicationException : Exception
{
    #region Constructors
    protected ApplicationException() { }

    protected ApplicationException(string message) : this(Array.Empty<object>(), message) { }
    
    protected ApplicationException(object[] propertyPathElements, string message)
    {
        Errors.Add(propertyPathElements, message);
    }
    #endregion
    
    #region Properties
    public Dictionary<object[], string> Errors { get; } = new();
    #endregion
}