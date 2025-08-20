using Humanizer;

namespace NATSInternal.Core.Services;

/// <summary>
/// A service to handle single-photo-related operations.
/// </summary>
/// <typeparam name="T">
/// The type of the entity with which the photos are associate.
/// </typeparam>
internal class SinglePhotoService<T> : IPhotoService<T> where T : class, IHasPhotosEntity<T>
{
    #region Fields
    private readonly string _rootPath;
    private readonly string _folderName = typeof(T).Name.Underscore();
    #endregion

    #region Constructors
    public SinglePhotoService(IPhotoStoragePathProvider pathProvider)
    {
        _rootPath = pathProvider.GetRootImageFolder();
    }
    #endregion

    #region Methods
    /// <inheritdoc />
    public async Task<string> CreateAsync(
            byte[] content,
            bool cropToSquare,
            CancellationToken cancellationToken = default)
    {
        MagickImage image = new MagickImage(content);

        // Process image's size
        ResizeImageIfTooLarge(image);
        if (cropToSquare)
        {
            CropIntoSquareImage(image);
        }

        // Determine the path where the image would be saved
        string path = Path.Combine(_rootPath, "images", _folderName);

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string fileName = DateTime.UtcNow.ToString("HH_mm_ss_fff__dd_MM_yyyy") + Guid.NewGuid() + ".jpg";
        string filePath = Path.Combine(path, fileName);
        await image.WriteAsync(filePath, cancellationToken);

        return $"/images/{_folderName}/{fileName}";
    }

    /// <inheritdoc />
    public async Task<string> CreateAsync(
            byte[] content,
            double aspectRatio,
            CancellationToken cancellationToken = default)
    {
        MagickImage image = new MagickImage(content);

        CropToAspectRatio(image, aspectRatio);

        // Determine the path where the image would be savedv
        string path = Path.Combine(_rootPath, "images");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        string fileName = DateTime.UtcNow.ToString("HH_mm_ss_fff__dd_MM_yyyy") + Guid.NewGuid() + ".jpg";
        string filePath = Path.Combine(path, fileName);
        await image.WriteAsync(filePath, cancellationToken);

        return $"/images/{_folderName}/{fileName}";
    }

    /// <inheritdoc />
    public void Delete(string relativePath)
    {
        List<string> pathElements = [
            _rootPath,
            .. relativePath.Split("/").Skip(1)
        ];
        string path = Path.Combine(pathElements.ToArray());

        if (!File.Exists(path))
        {
            throw new NotFoundException();
        }

        File.Delete(path);
    }

    /// <inheritdoc />
    public virtual async Task CreateMultipleAsync(
            T entity,
            List<PhotoRequestDto> requestDtos,
            CancellationToken cancellationToken = default)
    {
        foreach (PhotoRequestDto requestDto in requestDtos)
        {
            string url = await CreateAsync(requestDto.File, false, cancellationToken);
            Photo photo = new()
            {
                Url = url
            };

            entity.Photos.Add(photo);
        }
    }

    /// <inheritdoc />
    public virtual async Task<(List<string>, List<string>)> UpdateMultipleAsync(
            T entity,
            List<PhotoRequestDto> requestDtos,
            CancellationToken cancellationToken = default)
    {
        List<string> urlsToBeDeletedWhenSucceeds = new List<string>();
        List<string> urlsToBeDeletedWhenFails = new List<string>();
        for (int i = 0; i < requestDtos.Count; i++)
        {
            PhotoRequestDto requestDto = requestDtos[i];
            Photo photo;
            if (requestDto.Id.HasValue)
            {
                // Fetch the photo entity with the given id from the request.
                photo = entity.Photos.SingleOrDefault(op => op.Id == requestDto.Id)
                    ?? throw OperationException.NotFound(
                        new object[] { nameof(entity.Photos), i },
                        DisplayNames.Photo
                    );

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

                        string url = await CreateAsync(requestDto.File, true, cancellationToken);
                        photo.Url = url;

                        // Mark the created photo to be deleted later if the transaction fails.
                        urlsToBeDeletedWhenFails.Add(url);
                    }
                }
            }
            else
            {
                // Create new photo if the request doesn't have id.
                string url = await CreateAsync(requestDto.File, true, cancellationToken);
                photo = new()
                {
                    Url = url,
                };

                entity.Photos.Add(photo);

                // Mark the created photo to be deleted later if the transaction fails.
                urlsToBeDeletedWhenFails.Add(url);
            }
        }

        return (urlsToBeDeletedWhenSucceeds, urlsToBeDeletedWhenFails);
    }
    #endregion

    #region StaticMethods
    /// <summary>
    /// Resize an image to the desired aspect ratio.
    /// </summary>
    /// <remarks>
    /// The width or height, which one has greater value, will remain. The other's value will be calculated based on the
    /// desired aspect ratio. The resized image will also be converted into JPEG format.
    /// </remarks>
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
                (uint)Math.Round(croppedHeight));
            image.Crop(geometry);
            return;
        }

        double croppedWidth = image.Height * desiredAspectRatio;
        geometry = new MagickGeometry(
            (int)Math.Round((image.Width - croppedWidth) / 2),
            0,
            (uint)Math.Round(croppedWidth),
            image.Height);
        image.Crop(geometry);
    }

    /// <summary>
    /// Resize an image if either of width or height, or both of them, exceeds the maximum pixel value (1024px) while
    /// keeping the aspect ratio. The resized image will also be converted into JPEG format.
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
            uint maxWidth = 1024,
            uint maxHeight = 1024)
    {
        image.Quality = 100;
        image.Format = MagickFormat.Jpeg;
        double widthHeightRatio = (double)image.Width / image.Height;
        // Checking if image width or height or both exceeds maximum size
        if (image.Width > maxWidth || image.Height > maxHeight)
        {
            uint newWidth, newHeight;
            // Width is greater than height, cropping the left and the right sides of the image
            if (widthHeightRatio > 1)
            {
                newHeight = maxHeight;
                newWidth = (uint)Math.Round(newHeight * widthHeightRatio);
            }
            else
            {
                newWidth = maxWidth;
                newHeight = (uint)Math.Round(newWidth / widthHeightRatio);
            }

            image.Resize(newWidth, newHeight);
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
        if (image.Width == image.Height)
        {
            return;
        }

        uint size = Math.Min(image.Width, image.Height);
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
    #endregion
}