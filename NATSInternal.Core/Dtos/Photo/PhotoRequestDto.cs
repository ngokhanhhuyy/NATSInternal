namespace NATSInternal.Core.Dtos;

public class PhotoRequestDto
{
    #region Properties
    public required Guid? Id { get; set; }
    public required byte[] File { get; set; }
    public required bool HasBeenChanged { get; set; }
    public required bool HasBeenDeleted { get; set; }
    #endregion

    #region Methods
    public void TransformValues()
    {
        if (Id == Guid.Empty)
        {
            Id = null;
        }
    }
    #endregion
}