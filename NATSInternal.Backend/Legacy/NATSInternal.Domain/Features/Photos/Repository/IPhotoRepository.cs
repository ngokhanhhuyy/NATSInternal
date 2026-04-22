namespace NATSInternal.Domain.Features.Photos;

internal interface IPhotoRepository
{
    #region Methods
    Task<Photo?> GetSinglePhotoByIdAsync(Guid id, CancellationToken cancellationToken);
    
    Task<ICollection<Photo>> GetMultiplePhotosByProductIdsAsync(
        IEnumerable<Guid> productIds,
        CancellationToken cancellationToken);
    
    Task<ICollection<Photo>> GetMultiplePhotosByBrandIdsAsync(
        IEnumerable<Guid> brandIds,
        CancellationToken cancellationToken);
    #endregion
}
