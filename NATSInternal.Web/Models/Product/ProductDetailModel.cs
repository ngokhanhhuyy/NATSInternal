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
        StockingQuantity = responseDto.StockingQuantity;
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
    public string Name { get; }
    public string? Description { get; }
    public string Unit { get; }
    public long DefaultAmountBeforeVatPerUnit { get; }
    public int DefaultVatPercentagePerUnit { get; }
    public int StockingQuantity { get; }
    public bool IsForRetail { get; }
    public bool IsDiscontinued { get; }
    public DateTime CreatedDateTime { get; }
    public UserBasicModel CreatedUser { get; }
    public DateTime? LastUpdatedDateTime { get; }
    public UserBasicModel? LastUpdatedUser { get; }
    public BrandBasicModel? Brand { get; }
    public ProductCategoryBasicModel? Category { get; }
    public IReadOnlyList<PhotoBasicModel> Photos { get; }
    public ProductExistingAuthorizationModel Authorization { get; }
    #endregion
}