using NATSInternal.Domain.Seedwork;

namespace NATSInternal.Domain.Features.Photos;

internal class Photo : AbstractEntity
{
    #region Constructors
    public Photo(string url, bool isThumbnail, Guid? brandId, Guid? productId)
    {
        Url = url;
        IsThumbnail = isThumbnail;
        BrandId = brandId;
        ProductId = productId;
    }
    #endregion

    #region Properties
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Url { get; private set; }
    public bool IsThumbnail { get; private set; }
    public Guid? BrandId { get; private set; }
    public Guid? ProductId { get; private set; }
    #endregion
}