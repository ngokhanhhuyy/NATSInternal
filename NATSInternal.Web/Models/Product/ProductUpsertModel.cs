using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NATSInternal.Application.Localization;
using NATSInternal.Application.UseCases.Products;
using NATSInternal.Application.UseCases.Shared;
using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Web.Models;

public class ProductUpsertModel
{
    #region Constructors
    public ProductUpsertModel() { }

    public ProductUpsertModel(
        IEnumerable<ProductCategoryBasicResponseDto> categoryOptionResponseDtos,
        IEnumerable<BrandBasicResponseDto> brandOptionResponseDtos)
    {
        MapFromCategoryOptionResponseDtos(categoryOptionResponseDtos);
        MapFromBrandOptionResponseDtos(brandOptionResponseDtos);
    }

    public ProductUpsertModel(
        ProductGetDetailResponseDto responseDto,
        IEnumerable<ProductCategoryBasicResponseDto> categoryOptionResponseDtos,
        IEnumerable<BrandBasicResponseDto> brandOptbrandOptionResponseDtosions)
        : this(categoryOptionResponseDtos, brandOptbrandOptionResponseDtosions)
    {
        Id = responseDto.Id;
        Name = responseDto.Name;
        Unit = responseDto.Unit;
        DefaultAmountBeforeVatPerUnit = responseDto.DefaultAmountBeforeVatPerUnit;
        DefaultVatPercentagePerUnit = responseDto.DefaultVatPercentagePerUnit;
        IsForRetail = responseDto.IsForRetail;
        IsDiscontinued = responseDto.IsDiscontinued;

        if (responseDto.Stock is not null)
        {  
            Stock.StockingQuantity = responseDto.Stock.StockingQuantity;

            if (responseDto.Stock.ResupplyThresholdQuantity.HasValue)
            {
                Stock.ResupplyThresholdQuantity = responseDto.Stock.ResupplyThresholdQuantity.Value;
            }
        }

        if (responseDto.Category is not null)
        {
            CategoryName = responseDto.Category.Name;
        }

        if (responseDto.Brand is not null)
        {
            BrandId = responseDto.Brand.Id;
            Brand = new(responseDto.Brand);
        }
    }
    #endregion

    #region Properties
    [DisplayName(DisplayNames.Id)]
    public Guid Id { get; set; }
    
    [DisplayName(DisplayNames.Name)]
    [Required]
    [MaxLength(ProductContracts.NameMaxLength)]
    public string Name { get; set; } = string.Empty;
    
    [DisplayName(DisplayNames.Unit)]
    [Required]
    [MaxLength(ProductContracts.UnitMaxLength)]
    public string Unit { get; set; } = string.Empty;

    [DisplayName(DisplayNames.Description)]
    [MaxLength(ProductContracts.DescriptionMaxLength)]
    public string? Description { get; set; }
    
    [DisplayName(DisplayNames.DefaultAmountBeforeVatPerUnit)]
    [Required]
    [Range(0, long.MaxValue)]
    public long DefaultAmountBeforeVatPerUnit { get; set; }
    
    [DisplayName(DisplayNames.DefaultVatPercentagePerUnit)]
    [Required]
    [Range(0, 100)]
    public int DefaultVatPercentagePerUnit { get; set; }
    
    [DisplayName(DisplayNames.Category)]
    public string? CategoryName { get; set; }
    
    [DisplayName(DisplayNames.Brand)]
    public Guid? BrandId { get; set; }
    
    [DisplayName(DisplayNames.IsForRetail)]
    [Required]
    public bool IsForRetail { get; set; }
    
    [DisplayName(DisplayNames.IsDiscontinued)]
    [Required]
    public bool IsDiscontinued { get; set; }

    [DisplayName(DisplayNames.Stock)]
    public ProductUpsertStockModel Stock { get; set; } = new();
    
    [DisplayName(DisplayNames.Brand)]
    [BindNever]
    public BrandBasicModel? Brand { get; }

    [DisplayName(DisplayNames.Category)]
    [BindNever]
    public IReadOnlyList<ProductCategoryBasicModel> CategoryOptions { get; private set; } =
        new List<ProductCategoryBasicModel>().AsReadOnly();

    [DisplayName(DisplayNames.Category)]
    [BindNever]
    public IReadOnlyList<BrandBasicModel> BrandOptions { get; private set; } =
        new List<BrandBasicModel>().AsReadOnly();
    #endregion

    #region Methods
    public void MapFromCategoryOptionResponseDtos(IEnumerable<ProductCategoryBasicResponseDto> responseDtos)
    {
        CategoryOptions = responseDtos
            .Select(dto => new ProductCategoryBasicModel(dto))
            .ToList()
            .AsReadOnly();
    }

    public void MapFromBrandOptionResponseDtos(IEnumerable<BrandBasicResponseDto> responseDtos)
    {
        BrandOptions = responseDtos
            .Select(dto => new BrandBasicModel(dto))
            .ToList()
            .AsReadOnly();
    }

    public ProductCreateRequestDto ToCreateRequestDto()
    {
        ProductCreateRequestDto requestDto = new();
        MapToRequestDto(requestDto);

        return requestDto;
    }

    public ProductUpdateRequestDto ToUpdateRequestDto()
    {
        ProductUpdateRequestDto requestDto = new();
        MapToRequestDto(requestDto);
        requestDto.Id = Id;
        requestDto.IsDiscontinued = IsDiscontinued;

        return requestDto;
    }
    #endregion

    #region PrivateMethods
    private void MapToRequestDto(AbstractProductUpsertRequestDto requestDto)
    {
        requestDto.Name = Name;
        requestDto.Unit = Unit;
        requestDto.Description = Description;
        requestDto.DefaultAmountBeforeVatPerUnit = DefaultAmountBeforeVatPerUnit;
        requestDto.DefaultVatPercentagePerUnit = DefaultVatPercentagePerUnit;
        requestDto.IsForRetail = IsForRetail;
        requestDto.BrandId = BrandId;
        requestDto.CategoryName = CategoryName;
        requestDto.Stock = Stock.ToRequestDto();
    }
    #endregion
}

public class ProductUpsertStockModel
{
    #region Properties
    [DisplayName(DisplayNames.StockingQuantity)]
    [Range(0, int.MaxValue)]
    public int StockingQuantity { get; set; }
    
    [DisplayName(DisplayNames.ResupplyThresholdQuantity)]
    [Range(0, int.MaxValue)]
    public int ResupplyThresholdQuantity { get; set; }
    #endregion

    #region Methods
    public ProductUpsertStockRequestDto ToRequestDto()
    {
        return new()
        {
            StockingQuantity = StockingQuantity,
            ResupplyThresholdQuantity = ResupplyThresholdQuantity
        };
    }
    #endregion
}