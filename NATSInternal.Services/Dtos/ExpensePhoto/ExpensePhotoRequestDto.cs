namespace NATSInternal.Services.Dtos;

public class ExpensePhotoRequestDto : IPhotoRequestDto
{
    public int? Id { get; set; }
    public byte[] File { get; set; }
    public bool HasBeenChanged { get; set; }
    
    public void TransformValues()
    {
    }
}