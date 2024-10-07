namespace NATSInternal.Services.Dtos;

public class OrderPhotoRequestDto : IPhotoRequestDto
{
    public int? Id { get; set; }
    public byte[] File { get; set; }
    public bool HasBeenChanged { get; set; }
    public bool HasBeenDeleted { get; set; }

    public void TransformValues()
    {
        if (Id.Value == 0)
        { 
            Id = null;
        }
    }
}