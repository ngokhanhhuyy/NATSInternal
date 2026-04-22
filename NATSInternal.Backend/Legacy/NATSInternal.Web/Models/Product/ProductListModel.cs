using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NATSInternal.Application.Localization;
using NATSInternal.Application.UseCases.Products;

namespace NATSInternal.Web.Models;

public class ProductListModel : AbstractListModel<
    ProductListProductModel,
    ProductGetListRequestDto,
    ProductGetListResponseDto,
    ProductGetListProductResponseDto,
    ProductGetListRequestDto.FieldToSort>
{
    #region Properties
    public BrandListModel BrandList { get; set; } = new();
    public ProductCategoryListModel CategoryList { get; set; } = new();
    #endregion
    
    #region ProtectedMethods
    protected override void MapItemsFromResponseDtos(IEnumerable<ProductGetListProductResponseDto> responseDtos)
    {
        Items = responseDtos
            .Select(dto => new ProductListProductModel(dto))
            .ToList()
            .AsReadOnly();
    }
    #endregion
}

public class ProductListProductModel
{
    #region Constructors
    public ProductListProductModel(ProductGetListProductResponseDto responseDto)
    {
        Id = responseDto.Id;
        Name = responseDto.Name;
        Unit = responseDto.Unit;
        DefaultAmountBeforeVatPerUnit = responseDto.DefaultAmountBeforeVatPerUnit;
        DefaultVatPercentagePerUnit = responseDto.DefaultVatPercentagePerUnit;
        StockingQuantity = responseDto.StockingQuantity;
        IsResupplyNeeded = responseDto.IsResupplyNeeded;
        ThumbnailUrl = responseDto.ThumbnailUrl;
        Authorization = new(responseDto.Authorization);

        if (responseDto.Category is not null)
        {
            Category = new(responseDto.Category);
        }

        if (responseDto.Brand is not null)
        {
            Brand = new(responseDto.Brand);
        }
    }
    #endregion

    #region Properties
    public Guid Id { get; set; }

    [DisplayName(DisplayNames.Name)]
    public string Name { get; set; }

    [DisplayName(DisplayNames.Unit)]
    public string Unit { get; set; }
    
    [DisplayName(DisplayNames.DefaultAmountBeforeVatPerUnit)]
    [DisplayFormat(DataFormatString = "{0:N0}vnđ")]
    public long DefaultAmountBeforeVatPerUnit { get; set; }
    
    [DisplayName(DisplayNames.DefaultVatPercentagePerUnit)]
    [DisplayFormat(DataFormatString = "{0:N0}%")]
    public int DefaultVatPercentagePerUnit { get; set; }
    
    [DisplayName(DisplayNames.StockingQuantity)]
    public int StockingQuantity { get; set; }
    
    [DisplayName("Cần nhập hàng")]
    public bool IsResupplyNeeded { get; set; }
    
    [DisplayName(DisplayNames.Thumbnail)]
    public string? ThumbnailUrl { get; set; }
    
    [DisplayName(DisplayNames.ProductCategory)]
    public ProductCategoryBasicModel? Category { get; set; }
    
    [DisplayName(DisplayNames.Brand)]
    public BrandBasicModel? Brand { get; set; }

    public ProductExistingAuthorizationModel Authorization { get; set; }
    #endregion
}