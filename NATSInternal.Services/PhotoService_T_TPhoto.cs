using Microsoft.AspNetCore.Hosting;

namespace NATSInternal.Services;

/// <inheritdoc />
internal class PhotoService<T, TPhoto> : PhotoService<T>, IPhotoService<T, TPhoto>
    where T : class, IHasPhotoEntity<T, TPhoto>, new()
    where TPhoto : class, IPhotoEntity<TPhoto>, new()
{
    public PhotoService(IWebHostEnvironment environment) : base(environment)
    {
    }

    /// <inheritdoc />
    public virtual async Task CreateMultipleAsync<TRequestDto>(
            T entity,
            List<TRequestDto> requestDtos,
            Action<TPhoto, TRequestDto> initializer = null)
        where TRequestDto : IPhotoRequestDto
    {
        foreach (TRequestDto requestDto in requestDtos)
        {
            string url = await CreateAsync(requestDto.File, false);
            TPhoto photo = new TPhoto
            {
                Url = url
            };

            initializer?.Invoke(photo, requestDto);
            entity.Photos.Add(photo);
        }
    }

    /// <inheritdoc />
    public virtual async Task<(List<string>, List<string>)> UpdateMultipleAsync<TRequestDto>(
            T entity,
            List<TRequestDto> requestDtos,
            Action<TPhoto, TRequestDto> initializer = null,
            Action<TPhoto, TRequestDto> updateAssigner = null)
        where TRequestDto : IPhotoRequestDto
    {
        List<string> urlsToBeDeletedWhenSucceeds = new List<string>();
        List<string> urlsToBeDeletedWhenFails = new List<string>();
        for (int i = 0; i < requestDtos.Count; i++)
        {
            TRequestDto requestDto = requestDtos[i];
            TPhoto photo;
            if (requestDto.Id.HasValue)
            {
                // Fetch the photo entity with the given id from the request.
                photo = entity.Photos.SingleOrDefault(op => op.Id == requestDto.Id);

                // Ensure the photo entity exists.
                if (photo == null)
                {
                    string errorMessage = ErrorMessages.NotFound
                        .ReplacePropertyName(DisplayNames.Photo)
                        .ReplacePropertyName(DisplayNames.Id)
                        .ReplaceAttemptedValue(requestDto.Id.ToString());
                    throw new OperationException($"photos[{i}]", errorMessage);
                }

                // Perform the modification when the photo is marked to have been changed.
                if (requestDto.HasBeenChanged)
                { 
                    // Delete the photo if indicated.
                    if (requestDto.HasBeenDeleted)
                    {
                        // Mark the current url to be deleted later when the transaction
                        // succeeds.
                        urlsToBeDeletedWhenSucceeds.Add(photo.Url);
                        entity.Photos.Remove(photo);
                        continue;
                    }
                    
                    // Create new photo if the request contains new data for a new one.
                    if (requestDto.File != null)
                    {
                        // Mark the current url to be deleted later when the transaction
                        // succeeds.
                        urlsToBeDeletedWhenSucceeds.Add(photo.Url);
                        
                        string url = await CreateAsync(requestDto.File, true);
                        photo.Url = url;

                        // Mark the created photo to be deleted later if the transaction fails.
                        urlsToBeDeletedWhenFails.Add(url);
                    }

                    // Execute assigner if specified.
                    updateAssigner?.Invoke(photo, requestDto);
                }
            }
            else
            {
                // Create new photo if the request doesn't have id.
                string url = await CreateAsync(requestDto.File, true);
                photo = new TPhoto
                {
                    Url = url,
                };

                initializer?.Invoke(photo, requestDto);

                entity.Photos.Add(photo);

                // Mark the created photo to be deleted later if the transaction fails.
                urlsToBeDeletedWhenFails.Add(url);
            }
        }

        return (urlsToBeDeletedWhenSucceeds, urlsToBeDeletedWhenFails);
    }
}
