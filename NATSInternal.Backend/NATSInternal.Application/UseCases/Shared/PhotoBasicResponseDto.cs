using NATSInternal.Domain.Features.Photos;

namespace NATSInternal.Application.UseCases.Shared;

public class PhotoBasicResponseDto
{
    #region Constructors
    internal PhotoBasicResponseDto(Photo photo)
    {
        Id = photo.Id;
        Url = photo.Url;
        IsThumbnail = photo.IsThumbnail;
    }
    #endregion
    
    #region Properties
    public Guid Id { get; }
    public string Url { get; }
    public bool IsThumbnail { get; }
    #endregion
}