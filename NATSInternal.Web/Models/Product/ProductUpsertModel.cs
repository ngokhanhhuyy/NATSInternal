using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NATSInternal.Application.Localization;
using NATSInternal.Application.UseCases.Products;
using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Web.Models;

public class ProductUpsertModel
{
    #region Constructors
    public ProductUpsertModel() { }

    public ProductUpsertModel(ProductGetDetailResponseDto responseDto)
    {
        Name = responseDto.Name;
        Unit = responseDto.Name;
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
    
    [DisplayName(DisplayNames.DefaultAmountBeforeVatPerUnit)]
    [Required]
    [Range(0, long.MaxValue)]
    public long DefaultAmountBeforeVatPerUnit { get; set; }
    
    [DisplayName(DisplayNames.DefaultVatPercentagePerUnit)]
    [Required]
    [Range(0, 100)]
    public int DefaultVatPercentagePerUnit { get; set; }
    
    [DisplayName(DisplayNames.IsForRetail)]
    [Required]
    public bool IsForRetail { get; set; }
    
    [DisplayName(DisplayNames.IsDiscontinued)]
    [Required]
    public bool IsDiscontinued { get; set; }

    [DisplayName(DisplayNames.Stock)]
    public StockUpsertModel Stock { get; set; } = new();
    #endregion
}