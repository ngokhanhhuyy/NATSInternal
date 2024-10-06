namespace NATSInternal.Services.Dtos;

public class TreatmentPhotoRequestDto : IPhotoRequestDto
{
    public int? Id { get; set; }
    public byte[] File { get; set; }
    public TreatmentPhotoType Type { get; set; }
    public bool HasBeenChanged { get; set; }
    public bool HasBeenDeleted { get; set; }

    public void TransformValues()
    {
    }
}
