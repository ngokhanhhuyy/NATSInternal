namespace NATSInternal.Domain.Exceptions;

public class DomainException : Exception
{
    #region Constructors
    public DomainException(string? message = null) : base(message) { }
    #endregion
}