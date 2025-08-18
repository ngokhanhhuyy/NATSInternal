namespace NATSInternal.Core.Dtos;

public class PhotoResponseDto
{
    #region Properties
    public Guid Id { get; set; }
    public string Url { get; set; }
    #endregion

    #region Constructors
    internal PhotoResponseDto(Photo photo)
    {
        Id = photo.Id;
        Url = photo.Url;
    }
    #endregion
}