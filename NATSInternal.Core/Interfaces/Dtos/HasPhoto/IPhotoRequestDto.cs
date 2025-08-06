namespace NATSInternal.Core.Interfaces.Dtos;

public interface IPhotoRequestDto : IRequestDto
{
    int? Id { get; set; }
    byte[] File { get; set; }
    bool HasBeenChanged { get; set; }
    bool HasBeenDeleted { get; set; }
}