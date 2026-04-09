namespace NATSInternal.Core.Interfaces.Dtos;

public interface IHasPhotosDetailResponseDto
{
    #region Properties
    List<PhotoResponseDto> Photos { get; }
    #endregion
}