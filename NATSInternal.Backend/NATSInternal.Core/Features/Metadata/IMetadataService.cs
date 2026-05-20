namespace NATSInternal.Core.Features.Metadata;

public interface IMetadataService
{
    #region Methods
    Task<MetadataResponseDto> GetMetadataAsync();
    #endregion
}
