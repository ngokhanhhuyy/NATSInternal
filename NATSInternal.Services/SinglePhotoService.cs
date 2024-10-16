using Microsoft.AspNetCore.Hosting;

namespace NATSInternal.Services;

/// <summary>
/// A service to handle single-photo-related operations.
/// </summary>
/// <typeparam name="T">
/// The type of the entity with which the photos are associate.
/// </typeparam>
internal class SinglePhotoService<T> : ISinglePhotoService<T>
    where T : class, IHasSinglePhotoEntity<T>, new()
{
    private readonly IWebHostEnvironment _environment;
    protected readonly string _folderName = typeof(T).Name.PascalCaseToSnakeCase();

    public SinglePhotoService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    /// <inheritdoc />
    public async Task<string> CreateAsync(byte[] content, bool cropToSquare)
    {
        MagickImage image = new MagickImage(content);

        // Process image's size
        ResizeImageIfTooLarge(image);
        if (cropToSquare)
        {
            CropIntoSquareImage(image);
        }

        // Determine the path where the image would be saved
        string path = Path.Combine(
            _environment.WebRootPath,
            "images",
            _folderName);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string fileName = DateTime.UtcNow
            .ToString("HH_mm_ss_fff__dd_MM_yyyy") + Guid.NewGuid() + ".jpg";
        string filePath = Path.Combine(path, fileName);
        await image.WriteAsync(filePath);
        return $"/images/{_folderName}/{fileName}";
    }

    /// <inheritdoc />
    public async Task<string> CreateAsync(byte[] content, double aspectRatio)
    {
        MagickImage image = new MagickImage(content);

        CropToAspectRatio(image, aspectRatio);

        // Determine the path where the image would be savedv
        string path = Path.Combine(_environment.WebRootPath, "images");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string fileName = DateTime.UtcNow
            .ToString("HH_mm_ss_fff__dd_MM_yyyy") + Guid.NewGuid() + ".jpg";
        string filePath = Path.Combine(path, fileName);
        await image.WriteAsync(filePath);
        return $"/images/{_folderName}/{fileName}";
    }

    /// <inheritdoc />
    public void Delete(string relativePath)
    {
        List<string> pathElements = [
            _environment.WebRootPath,
            .. relativePath.Split("/").Skip(1)
        ];
        string path = Path.Combine(pathElements.ToArray());

        if (!File.Exists(path))
        {
            throw new ResourceNotFoundException(relativePath);
        }

        File.Delete(path);
    }

    /// <summary>
    /// Resize an image if either of width or height, or both of them, exceeds the maximum
    /// pixel value (1024px) while keeping the aspect ratio.
    /// The resized image will also be converted into JPEG format.
    /// </summary>
    /// <param name="image">
    /// An IMagickImage instance loaded from byte array to be checked and resized.
    /// </param>
    /// <param name="maxWidth">
    /// The maximum width the of the photo that will trigger the resizing of exceeded.
    /// </param>
    /// <param name="maxHeight">
    /// The maximum height the of the photo that will trigger the resizing of exceeded.
    /// </param>
    private static void ResizeImageIfTooLarge(
            MagickImage image,
            int maxWidth = 1024,
            int maxHeight = 1024)
    {
        image.Quality = 100;
        image.Format = MagickFormat.Jpeg;
        double widthHeightRatio = (double)image.Width / image.Height;
        // Checking if image width or height or both exceeds maximum size
        if (image.Width > maxWidth || image.Height > maxHeight)
        {
            int newWidth, newHeight;
            // Width is greater than height, cropping the left and the right sides of the image
            if (widthHeightRatio > 1)
            {
                newHeight = maxHeight;
                newWidth = (int)Math.Round(newHeight * widthHeightRatio);
            }
            else
            {
                newWidth = maxWidth;
                newHeight = (int)Math.Round(newWidth / widthHeightRatio);
            }
            image.Resize(newWidth, newHeight);
        }
    }

    /// <summary>
    /// Resize an image to the desired aspect ratio.
    /// The width or height, which one has greater value, will remain.
    /// The other's value will be calculated based on the desired aspect ratio.
    /// The resized image will also be converted into JPEG format.
    /// </summary>
    /// <param name="image">
    /// An IMagickImage instance loaded from byte array to be checked and resized.
    /// </param>
    /// <param name="desiredAspectRatio">
    /// The desired aspect ratio of the image after being cropped.
    /// </param>
    private static void CropToAspectRatio(MagickImage image, double desiredAspectRatio)
    {
        double originalAspectRatio = (double)image.Width / image.Height;
        // Determine which one of width and height is larger.
        MagickGeometry geometry;
        if (desiredAspectRatio >= originalAspectRatio)
        {
            double croppedHeight = image.Width / desiredAspectRatio;
            geometry = new MagickGeometry(
                0,
                (int)Math.Round((image.Height - croppedHeight) / 2),
                image.Width,
                (int)Math.Round(croppedHeight));
            image.Crop(geometry);
        }
        else
        {
            double croppedWidth = image.Height * desiredAspectRatio;
            geometry = new MagickGeometry(
                (int)Math.Round((image.Width - croppedWidth) / 2),
                0,
                (int)Math.Round(croppedWidth),
                image.Height);
            image.Crop(geometry);
        }
    }

    /// <summary>
    /// Crop an image into square. The geomery of the part which is kept after cropping is the center of the image.
    /// The size after being cropped will equal to original image's width or height, based on which one is smaller.
    /// </summary>
    /// <param name="image">
    /// An IMagickImage instance loaded from byte array to be checked and cropped.
    /// </param>
    private static void CropIntoSquareImage(MagickImage image)
    {
        // Crop image if needed to make sure it's square
        if (image.Width != image.Height)
        {
            int size = Math.Min(image.Width, image.Height);
            int x, y;
            if (image.Width > image.Height)
            {
                x = (int)Math.Round((double)(image.Width - image.Height) / 2);
                y = 0;
            }
            else
            {
                x = 0;
                y = (int)Math.Round((double)(image.Height - image.Width) / 2);
            }
            image.Crop(new MagickGeometry(x, y, size, size));
        }
    }
}

/// <summary>
/// A service to handle both single-photo and multiple-photo related operations.
/// </summary>
/// <typeparam name="T">
/// The type of the entity with which the photos are associate.
/// </typeparam>
/// The type of the photo entity, ass
/// <typeparam name="TPhoto">ociated to the <see cref="T"/> entity.
/// </typeparam>
internal class MultiplePhotosService<T, TPhoto>
    :
        SinglePhotoService<T>,
        IMultiplePhotosService<T, TPhoto>
    where T : class, IHasMultiplePhotosEntity<T, TPhoto>, new()
    where TPhoto : class, IPhotoEntity<TPhoto>, new()
{
    public MultiplePhotosService(IWebHostEnvironment environment) : base(environment)
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