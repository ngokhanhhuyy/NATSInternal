namespace NATSInternal.Core.Dtos;

public class SupplyPhotoRequestDto : IPhotoRequestDto
{
    public int? Id { get; set; }
    public byte[] File { get; set; }
    public bool HasBeenChanged { get; set; }
    public bool HasBeenDeleted { get; set; }

    public void TransformValues()
    {
        Id = Id == 0 ? null : Id;
    }
}
