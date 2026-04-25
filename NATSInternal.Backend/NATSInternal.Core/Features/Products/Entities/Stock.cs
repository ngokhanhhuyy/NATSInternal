using System.ComponentModel.DataAnnotations;

namespace NATSInternal.Core.Features.Products;

internal class Stock
{
    #region Properties
    [Key]
    public int Id { get; private set; }

    [Required]
    public int StockingQuantity { get; set; }
    
    [Required]
    public int? ResupplyThresholdQuantity { get; set; }
    #endregion
    
    #region ForeignKeyProperties
    [Required]
    public int ProductId { get; set; }
    #endregion

    #region NavigationProperties
    public Product Product { get; set; } = null!;
    #endregion
}