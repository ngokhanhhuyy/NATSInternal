namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IHasPhotoDetailResponseDto<TPhoto> where TPhoto : IPhotoResponseDto
{
    List<TPhoto> Photos { get; internal set; }
}