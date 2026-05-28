using NATSInternal.Core.Common.Entities;
using NATSInternal.Core.Features.Products;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NATSInternal.Core.Features.Orders;

internal class OrderProductItem : IHasProductItemEntity
{
    #region Properties
    [Key]
    public int Id { get; private set; }

    [Required]
    public long AmountBeforeVatPerUnit { get; set; }

    [Required]
    public int VatPercentagePerUnit { get; set; }

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

    #region ComputedProperties
    [NotMapped]
    public long VatAmountPerUnit => (long)Math.Ceiling(AmountBeforeVatPerUnit * (VatPercentagePerUnit / 100M));

    [NotMapped]
    public long VatAmount => VatAmountPerUnit * Quantity;

    [NotMapped]
    public long AmountAfterVat => (AmountBeforeVatPerUnit * Quantity) + VatAmount;
    #endregion
}
