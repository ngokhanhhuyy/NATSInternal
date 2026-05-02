using NATSInternal.Core.Features.Products;
using System.ComponentModel.DataAnnotations;
using NATSInternal.Core.Common.Entities;

namespace NATSInternal.Core.Features.Orders;

internal class OrderProductItem : IHasProductItemEntity
{
    #region Properties
    [Key]
    public int Id { get; private set; }

    [Required]
    public long AmountBeforeVatPerUnit { get; set; }

    [Required]
    public long VatAmountPerUnit { get; set; }

    [Required]
    public int Quantity { get; set; } = 1;
    #endregion

    #region ForeignKeyProperties
    [Required]
    public int OrderId { get; set; }

    public int ProductId { get; set; }
    #endregion

    #region NavigationProperties
    public Order Order { get; set; } = null!;
    public Product Product { get; set; } = null!;
    #endregion
}