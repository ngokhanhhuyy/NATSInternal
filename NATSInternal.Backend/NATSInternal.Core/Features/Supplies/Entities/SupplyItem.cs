using NATSInternal.Core.Common.Entities;
using NATSInternal.Core.Features.Products;
using System.ComponentModel.DataAnnotations;

namespace NATSInternal.Core.Features.Supplies;

internal class SupplyItem : IHasProductItemEntity
{
    #region Properties
    [Key]
    public int Id { get; private set; }

    [Required]
    public long AmountPerUnit { get; set; }

    [Required]
    public int Quantity { get; set; }
    #endregion
    
    #region ForeignKeyProperties
    [Required]
    public int SupplyId { get; set; }

    [Required]
    public int ProductId { get; set; }
    #endregion

    #region NavigationProperties
    public Supply Supply { get; set; } = null!;
    public Product Product { get; set; } = null!;
    #endregion
}