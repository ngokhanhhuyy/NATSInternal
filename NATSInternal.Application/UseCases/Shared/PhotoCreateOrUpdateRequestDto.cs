namespace NATSInternal.Application.UseCases.Shared;

public class PhotoCreateOrUpdateRequestDto : IRequestDto
{
    #region Properties
    public required Guid? Id { get; set; }
    public required byte[] File { get; set; }
    public required bool IsThumbnail { get; set; }
    public required bool IsChanged { get; set; }
    public required bool IsDeleted { get; set; }
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