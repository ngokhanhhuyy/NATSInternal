namespace NATSInternal.Core.Common.Exceptions;

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
    public Dictionary<object[], string> Errors { get; private set; } = new();
    #endregion

    #region Methods
    public void AddPropertyPathElementToTheBeginning(IEnumerable<object> propertyPathElements)
    {
        Errors = Errors
            .Select(p =>
            {
                List<object> modifiedPropertyPathElements = propertyPathElements.ToList();
                modifiedPropertyPathElements.AddRange(p.Key);

                return new
                {
                    PropertyPathElements = modifiedPropertyPathElements.ToArray(),
                    Message = p.Value
                };
            })
            .ToDictionary(p => p.PropertyPathElements, p => p.Message);
    }
    #endregion
}