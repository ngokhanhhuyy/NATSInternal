namespace NATSInternal.Core.Features.Photos;

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
    public int Id { get; }
    public string Url { get; }
    public bool IsThumbnail { get; }
    #endregion
}