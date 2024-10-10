namespace NATSInternal.Services.Interfaces.Dtos;

public interface IHasPhotoDetailResponseDto : IHasPhotoBasicResponseDto;

public interface IHasPhotoDetailResponseDto<TPhoto> : IHasPhotoDetailResponseDto
    where TPhoto : IPhotoResponseDto
{
    List<TPhoto> Photos { get; }
}