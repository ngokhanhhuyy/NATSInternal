namespace NATSInternal.Services.Dtos;

public class UserAvatarUpsertRequestDto : IRequestDto
{
    public bool HasChanged { get; set; }
    public byte[] Content { get; set; }

    public void TransformValues()
    {
    }
}