namespace NATSInternal.Core.Exceptions;

public class DuplicatedException(string propertyName)
        : Exception(ErrorMessages.UniqueDuplicated.Replace(
            "{PropertyName}",
            DisplayNames.Get(propertyName)))
{
    public string PropertyName { get; set; } = propertyName;
}
