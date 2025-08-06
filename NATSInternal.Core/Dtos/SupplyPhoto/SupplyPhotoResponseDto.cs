namespace NATSInternal.Core.Dtos;

public class SupplyPhotoResponseDto : IPhotoResponseDto
{
    public int Id { get; set; }
    public string Url { get; set; }

    internal SupplyPhotoResponseDto(SupplyPhoto photo)
    {
        Id = photo.Id;
        Url = photo.Url;
    }
}
