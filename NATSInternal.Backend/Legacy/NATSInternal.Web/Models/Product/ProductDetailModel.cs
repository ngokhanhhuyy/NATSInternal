using System.ComponentModel;
using NATSInternal.Application.Localization;
using NATSInternal.Application.UseCases.Products;
using NATSInternal.Web.Models.Shared;

namespace NATSInternal.Web.Models;

public class ProductDetailModel
{
    #region Constructors
    public ProductDetailModel(ProductGetDetailResponseDto responseDto)
    {
        Id = responseDto.Id;
        Name = responseDto.Name;
        Description = responseDto.Description;
        Unit = responseDto.Unit;
        DefaultAmountBeforeVatPerUnit = responseDto.DefaultAmountBeforeVatPerUnit;
        DefaultVatPercentagePerUnit = responseDto.DefaultVatPercentagePerUnit;
        IsForRetail = responseDto.IsForRetail;
        IsDiscontinued = responseDto.IsDiscontinued;
        CreatedDateTime = responseDto.CreatedDateTime;
        CreatedUser = new(responseDto.CreatedUser);
        LastUpdatedDateTime = responseDto.LastUpdatedDateTime;
        Photos = responseDto.Photos.Select(p => new PhotoBasicModel(p)).ToList().AsReadOnly();
        Authorization = new(responseDto.Authorization);

        if (responseDto.LastUpdatedDateTime.HasValue)
        {
            LastUpdatedDateTime = responseDto.LastUpdatedDateTime;
        }

        if (responseDto.LastUpdatedUser is not null)
        {
            LastUpdatedUser = new(responseDto.LastUpdatedUser);
        }

        if (responseDto.Stock is not null)
        {
            Stock = new(responseDto.Stock);
        }

        if (responseDto.Brand is not null)
        {
            Brand = new(responseDto.Brand);
        }

        if (responseDto.Category is not null)
        {
            Category = new(responseDto.Category);
        }
    }
    #endregion

    #region Properties
    public Guid Id { get; }

    [DisplayName(DisplayNames.Name)]
    public string Name { get; }

    [DisplayName(DisplayNames.Description)]
    public string? Description { get; }

    [DisplayName(DisplayNames.Unit)]
    public string Unit { get; }

    [DisplayName(DisplayNames.DefaultAmountBeforeVatPerUnit)]
    public long DefaultAmountBeforeVatPerUnit { get; }

    [DisplayName(DisplayNames.DefaultVatPercentagePerUnit)]
    public int DefaultVatPercentagePerUnit { get; }

    [DisplayName(DisplayNames.IsForRetail)]
    public bool IsForRetail { get; }

    [DisplayName(DisplayNames.IsDiscontinued)]
    public bool IsDiscontinued { get; }

    [DisplayName(DisplayNames.CreatedDateTime)]
    public DateTime CreatedDateTime { get; }

    [DisplayName(DisplayNames.CreatedUser)]
    public UserBasicModel CreatedUser { get; }

    [DisplayName(DisplayNames.LastUpdatedDateTime)]
    public DateTime? LastUpdatedDateTime { get; }

    [DisplayName(DisplayNames.LastUpdatedUser)]
    public UserBasicModel? LastUpdatedUser { get; }

    [DisplayName(DisplayNames.Stock)]
    public StockBasicModel? Stock { get; }

    [DisplayName(DisplayNames.Name)]
    public BrandBasicModel? Brand { get; }

    [DisplayName(DisplayNames.Name)]
    public ProductCategoryBasicModel? Category { get; }

    [DisplayName(DisplayNames.Name)]
    public IReadOnlyList<PhotoBasicModel> Photos { get; }

    [DisplayName(DisplayNames.Thumbnail)]
    public string? ThumbnailUrl => Photos.Where(p => p.IsThumbnail).Select(p => p.Url).FirstOrDefault();

    public ProductExistingAuthorizationModel Authorization { get; }
    #endregion
}