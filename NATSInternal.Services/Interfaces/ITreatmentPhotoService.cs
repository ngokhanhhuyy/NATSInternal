namespace NATSInternal.Services.Interfaces;

/// <summary>
/// A service to handle treatment-photo-related operations.
/// </summary>
internal interface ITreatmentPhotoService : IMultiplePhotosService<Treatment, TreatmentPhoto>
{
    /// <summary>
    /// Create photos which are associated to the specified treatment with the data provided
    /// in the request.
    /// </summary>
    /// <param name="treatment">
    /// An instance of the <typeparamref name="Treatment"/> entity to which the photos are associated.
    /// </param>
    /// <param name="requestDtos">
    /// A <see cref="List{TRequestDto}"/> of photo DTOs, containing the data of the photos to
    /// be created.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> reprensenting the asynchronous operation.
    /// </returns>
    Task CreateMultipleAsync<TRequestDto>(Treatment treatment, List<TRequestDto> requestDtos)
        where TRequestDto : TreatmentPhotoRequestDto;
    
    /// <summary>
    /// Update the specified entity's photos with the data provided in the request.
    /// </summary>
    /// <param name="treatment">
    /// The instance of the <typeparamref name="Treatment"/> entity to which the updating photos are
    /// associated.
    /// </param>
    /// <param name="requestDtos">
    /// An object containing the data for the photos to be updated.
    /// </param>
    /// <returns>
    /// A <typeparamref name="Tuple"/> containing 2 lists of strings. The first one contains the urls
    /// of the photos which must be deleted when the update operation succeeded. The
    /// other one contains the urls of the photos which must be deleted when the
    /// updating operation failed.
    /// </returns>
    /// <exception cref="OperationException">
    /// Thrown when the photo with the given id which is associated to the specified
    /// entity in the request cannot be found.
    /// </exception>
    Task<(List<string>, List<string>)> UpdateMultipleAsync<TRequestDto>(
            Treatment treatment,
            List<TRequestDto> requestDtos)
        where TRequestDto : TreatmentPhotoRequestDto;
}