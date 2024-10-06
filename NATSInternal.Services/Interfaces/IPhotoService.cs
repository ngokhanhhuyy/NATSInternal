namespace NATSInternal.Services.Interfaces;

/// <summary>
/// A service to handle the photo-related operations.
/// </summary>
/// <typeparam name="T">T
/// he type of the entity which is associated the photos.
/// </typeparam>
/// <typeparam name="TPhoto">
/// The type of the photo entity.
/// </typeparam>
internal interface IPhotoService<in T, out TPhoto>
    where T : class, IHasPhotoEntity<T, TPhoto>, new()
    where TPhoto : class, IPhotoEntity<TPhoto>, new()
{
    /// <summary>
    /// Creates a new photo and save it into a specific folder.
    /// </summary>
    /// <remarks>
    /// The folder which name is specified by the value of the <c>folderName</c> argument is
    /// placed under the directory <c>/wwwroot/photos</c>. The name of the photo file is a
    /// string which contains the combination of the created datetime and a UUID.
    /// </remarks>
    /// <returns>
    /// <param name="content">
    /// An <see cref="Array"/> of <see cref="byte"/>, contanining the data of the photo after
    /// reading the file provided in the request.
    /// </param>
    /// <param name="cropToSquare">
    /// A <see cref="bool"/> value indicating if the image should be cropped into square image.
    /// </param>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is a string
    /// representing the relative path (URL) to the created photo in the specified folder.
    /// </returns>
    Task<string> CreateAsync(byte[] content, bool cropToSquare);

    /// <summary>
    /// Creates a new photo.
    /// </summary>
    /// <remarks>
    /// The folder in which the photo is stored has name determined by the name of the
    /// <see cref="T" /> entity (converted into snake case) and placed under the directory
    /// <c>/wwwroot/photos</c>. The name of the photo file is a string which contains the
    /// combination of the created datetime and a UUID.
    /// </remarks>
    /// <param name="content">
    /// An <see cref="Array"/> of <see cref="byte"/>, contanining the data of the photo after
    /// reading the file provided in the request.
    /// </param>
    /// <param name="aspectRatio">
    /// A <see cref="double"/> value indicate the aspect ratio that the operation should base
    /// on to crop the photo.
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> representing the asynchronous operation, which result is a string
    /// representing the relative path (URL) to the created photo in the specified folder.
    /// </returns>
    Task<string> CreateAsync(byte[] content, double aspectRatio);

    /// <summary>
    /// Deletes an existing photo by the relative path on the server.
    /// </summary>
    /// <param name="relativePath">
    /// A <see cref="string"/> representing the full path to the photo on the server, usually
    /// in <c>wwwroot/photos/{entityName}/</c>
    /// </param>
    void Delete(string relativePath);

    /// <summary>
    /// Create photos which are associated to the specified entity with the data
    /// provided in the request.
    /// </summary>
    /// <param name="entity">
    /// The <see cref="T"/> entity to which the photos are associated.
    /// </param>
    /// <param name="requestDtos">
    /// A <see cref="List{T}"/> of photo DTOs, containing the data of the photos to be created.
    /// </param>
    /// <param name="initializer">
    /// An action which will be executed during the intialization of the <see cref="TPhoto"/>
    /// entity.
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
    /// An action which will be executed during the intialization of the <see cref="TPhoto"/>
    /// of a new entity.
    /// </param>
    /// <param name="updateAssigner">
    /// An action which will be executed during data assignment from the associated DTO to each
    /// entity when the entity is indicated to be updated (not deleted).
    /// of a new entity.
    /// </param>
    /// <returns>
    /// A <see cref="Tuple"/> containing 2 lists of strings. The first one contains the urls
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
            Action<TPhoto, TRequestDto> initializer,
            Action<TPhoto, TRequestDto> updateAssigner = null)
        where TRequestDto : IPhotoRequestDto;
}