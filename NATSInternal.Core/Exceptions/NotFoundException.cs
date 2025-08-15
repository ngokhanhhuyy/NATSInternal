namespace NATSInternal.Core.Exceptions;

public class ResourceNotFoundException : CoreException
{
    public ResourceNotFoundException() { }
    
    public
    
    public ResourceNotFoundException(string message) : base(message) { }

    public ResourceNotFoundException(string resourceName, string propertyName, string attemptedValue)
        : base(ErrorMessages.NotFoundByProperty
            .Replace("{ResourceName}", DisplayNames.Get(resourceName))
            .Replace("{PropertyName}", DisplayNames.Get(propertyName))
            .Replace("{AttemptedValue}", attemptedValue))
    {
        ResourceName = resourceName;
        PropertyName = propertyName;
        AttemptedValue = attemptedValue;
    }

    public string ResourceName { get; set; }
    public string PropertyName { get; set; }
    public object AttemptedValue { get; set; }
}
