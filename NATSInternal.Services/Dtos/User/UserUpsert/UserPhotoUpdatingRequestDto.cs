namespace NATSInternal.Services.Dtos;

public class UserPhotoUpdatingRequestDto : IRequestDto
{
    public string Operation { get; set; }
    public int? Id { get; set; }
    public byte[] Content { get; set; }

    public void TransformValues()
    {
    }

    public enum OperationName
    {
        Create,
        Replace,
        Delete
    }

}
