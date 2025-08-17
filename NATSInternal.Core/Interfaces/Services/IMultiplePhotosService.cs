namespace NATSInternal.Core.Interfaces.Services;

/// <inheritdoc />
/// <typeparam name="T">
/// The type of the entity which is associated the photos.
/// </typeparam>
/// <typeparam name="TPhoto">
/// The type of the photo entity.
/// </typeparam>
internal interface IMultiplePhotosService<T, out TPhoto> : ISinglePhotoService<T>
    where T : class, IHasPhotosEntity<T, TPhoto>, new()
    where TPhoto : class, IPhotoEntity<TPhoto>, new()
{
    /// <summary>
    /// Create photos which are associated to the specified entity with the data
    /// provided in the request.
    /// </summary>
    /// <param name="entity">
    /// The <typeparamref name="T"/> entity to which the photos are associated.
    /// </param>
    /// <param name="requestDtos">
    /// A <see cref="List{T}"/> of photo DTOs, containing the data of the photos to be created.
    /// </param>
    /// <param name="initializer">
    /// (Optional) An action which will be executed during the intialization of the
    /// <typeparamref name="TPhoto"/> entity.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> reprensenting the asynchronous operation.
    /// </returns>
    Task CreateMultipleAsync<TRequestDto>(
            T entity,
            List<TRequestDto> requestDtos,
            Action<TPhoto, TRequestDto> initializer = null)
        where TRequestDto : IPhotoRequestDto;

    /// <summary>
    /// Update the specified entity's photos with the data provided in the request.
    /// </summary>
    /// <param name="entity">
    /// The entity to which the updating photos are associated.
    /// </param>
    /// <param name="requestDtos">
    /// An object containing the data for the photos to be updated.
    /// </param>
    /// <param name="initializer">
    /// (Optional) An action which will be executed during the intialization of the
    /// <typeparamref name="TPhoto"/> of a new entity.
    /// </param>
    /// <param name="updateAssigner">
    /// (Optional) An action which will be executed during data assignment from the associated
    /// DTO to each entity when the entity is indicated to be updated (not deleted).
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
            T entity,
            List<TRequestDto> requestDtos,
            Action<TPhoto, TRequestDto> initializer = null,
            Action<TPhoto, TRequestDto> updateAssigner = null)
        where TRequestDto : IPhotoRequestDto;
}