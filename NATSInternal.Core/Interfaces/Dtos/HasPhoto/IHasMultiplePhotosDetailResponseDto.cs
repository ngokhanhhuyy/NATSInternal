namespace NATSInternal.Core.Interfaces.Dtos;

public interface IHasMultiplePhotosDetailResponseDto<TPhoto> where TPhoto : IPhotoResponseDto
{
    List<TPhoto> Photos { get; }
}