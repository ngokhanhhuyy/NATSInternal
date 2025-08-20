namespace NATSInternal.Core.Interfaces.Services;

/// <summary>
/// A service to handle the photo-related operations.
/// </summary>
internal interface IPhotoService<T> where T : class, IHasPhotosEntity<T>
{
    /// <summary>
    /// Creates a new photo and save it into a specific folder.
    /// </summary>
    /// <remarks>
    /// The folder which name is specified by the value of the <paramref name="folderName"/> is placed under the
    /// directory <c>/wwwroot/photos</c>. The name of the photo file is a string which contains the combination of the
    /// created datetime and a UUID.
    /// </remarks>
    /// <returns>
    /// <param name="content">
    /// An <see cref="Array"/> of <see cref="byte"/>, contanining the data of the photo after reading the file provided
    /// in the request.
    /// </param>
    /// <param name="cropToSquare">
    /// A <see cref="bool"/> value indicating if the image should be cropped into square image.
    /// </param>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is a string representing the relative
    /// path (URL) to the created photo in the specified folder.
    /// </returns>
    Task<string> CreateAsync(
            byte[] content,
            bool cropToSquare,
            CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates a new photo.
    /// </summary>
    /// <remarks>
    /// The folder in which the photo is stored has name determined by the name of the <typeparamref name="T"/> entity
    /// (converted into snake case) and placed under the directory <c>/wwwroot/photos</c>. The name of the photo file is
    /// a string which contains the combination of the created datetime and a UUID.
    /// </remarks>
    /// <param name="content">
    /// An <see cref="Array"/> of <see cref="byte"/>, contanining the data of the photo after reading the file provided
    /// in the request.
    /// </param>
    /// <param name="aspectRatio">
    /// A <see cref="double"/> value indicate the aspect ratio that the operation should base on to crop the photo.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is a string representing the relative
    /// path (URL) to the created photo in the specified folder.
    /// </returns>
    Task<string> CreateAsync(
            byte[] content,
            double aspectRatio,
            CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an existing photo by the relative path on the server.
    /// </summary>
    /// <param name="relativePath">
    /// A <see cref="string"/> representing the full path to the photo on the server, usually in
    /// <c>wwwroot/photos/{entityName}/</c>
    /// </param>
    void Delete(string relativePath);

    /// <summary>
    /// Create photos which are associated to the specified entity with the data provided in the request.
    /// </summary>
    /// <param name="entity">
    /// The <typeparamref name="T"/> entity to which the photos are associated.
    /// </param>
    /// <param name="requestDtos">
    /// A <see cref="List{T}"/> of photo DTOs, containing the data of the photos to be created.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> reprensenting the asynchronous operation.
    /// </returns>
    Task CreateMultipleAsync(
            T entity,
            List<PhotoRequestDto> requestDtos,
            CancellationToken cancellationToken = default);

    /// <summary>
    /// Update the specified entity's photos with the data provided in the request.
    /// </summary>
    /// <param name="requestDtos">
    /// An object containing the data for the photos to be updated.
    /// </param>
    /// <param name="updateAssigner">
    /// (Optional) An action which will be executed during data assignment from the associated DTO to each entity when
    /// the entity is indicated to be updated (not deleted).
    /// </param>
    /// <returns>
    /// A <see cref="Tuple"/> containing 2 lists of strings. The first one contains the urls of the photos which must be
    /// deleted when the update operation succeeded. The other one contains the urls of the photos which must be deleted
    /// when the updating operation failed.
    /// </returns>
    /// <exception cref="OperationException">
    /// Thrown when the photo with the given id which is associated to the specified entity in the request cannot be
    /// found.
    /// </exception>
    Task<(List<string>, List<string>)> UpdateMultipleAsync(
            T entity,
            List<PhotoRequestDto> requestDtos,
            CancellationToken cancellationToken = default);
}