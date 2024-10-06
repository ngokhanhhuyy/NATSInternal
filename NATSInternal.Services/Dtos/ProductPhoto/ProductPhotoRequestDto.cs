namespace NATSInternal.Services.Dtos;

public class ProductPhotoRequestDto : IPhotoRequestDto<ProductPhotoRequestDto>
{
    public int? Id { get; set; }
    public byte[] File { get; set; }
    public bool HasBeenChanged { get; set; }

    public void TransformValues()
    {
        Id = Id.HasValue && Id.Value == 0 ? null : Id;
    }
}