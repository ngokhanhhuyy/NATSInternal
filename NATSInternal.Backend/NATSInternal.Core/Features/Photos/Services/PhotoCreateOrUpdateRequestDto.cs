using NATSInternal.Core.Common.Dtos;

namespace NATSInternal.Core.Features.Photos;

public class PhotoCreateOrUpdateRequestDto : IRequestDto
{
    #region Properties
    public required int? Id { get; set; }
    public required byte[] File { get; set; }
    public required bool IsThumbnail { get; set; }
    public required bool IsChanged { get; set; }
    public required bool IsDeleted { get; set; }
    #endregion

    #region Methods
    public void TransformValues()
    {
        if (Id == 0)
        {
            Id = null;
        }
    }
    #endregion
}