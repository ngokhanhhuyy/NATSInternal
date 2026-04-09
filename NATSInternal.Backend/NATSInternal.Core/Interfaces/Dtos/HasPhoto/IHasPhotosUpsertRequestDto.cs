namespace NATSInternal.Core.Interfaces.Dtos;

public interface IHasPhotosUpsertRequestDto : IRequestDto
{
    #region Properties
    List<PhotoRequestDto> Photos { get; set; }
    #endregion
}
