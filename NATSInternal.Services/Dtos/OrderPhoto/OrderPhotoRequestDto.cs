namespace NATSInternal.Services.Dtos;

public class OrderPhotoRequestDto : IRequestDto
{
    public int? Id { get; set; }
    public byte[] File { get; set; }
    public bool HasBeenChanged { get; set; }
        
    public void TransformValues()
    {
            if (Id.HasValue && Id.Value == 0)
            { 
                Id = null;
            }
        }
}