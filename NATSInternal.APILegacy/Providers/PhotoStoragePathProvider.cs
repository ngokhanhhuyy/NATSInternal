namespace NATSInternal.API.Providers;

public class PhotoStoragePathProvider : IPhotoStoragePathProvider
{
    #region Fields
    private readonly IWebHostEnvironment _environment;
    #endregion

    #region Constructors
    public PhotoStoragePathProvider(IWebHostEnvironment environment)
    {
        _environment = environment;
    }
    #endregion

    #region Methods
    public string GetRootImageFolder()
    {
        return _environment.WebRootPath;
    }
    #endregion
}