namespace NATSInternal.Core.Dtos;

public class OrderPhotoResponseDto : IPhotoResponseDto
{
    public int Id { get; set; }
    public string Url { get; set; }

    internal OrderPhotoResponseDto(Photo photo)
    {
        Id = photo.Id;
        Url = photo.Url;
    }
}