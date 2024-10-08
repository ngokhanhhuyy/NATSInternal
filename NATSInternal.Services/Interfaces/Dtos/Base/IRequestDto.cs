namespace NATSInternal.Services.Interfaces.Dtos;

/// <summary>
/// A DTO class containing the data mapped from the requests for data-related operations.
/// </summary>
public interface IRequestDto
{
    /// <summary>
    /// Transform the values of all properties which represent the absence of data or meanless
    /// value into the default value.
    /// </summary>
    public void TransformValues()
    {
    }
}