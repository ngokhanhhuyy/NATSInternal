using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NATSInternal.Core.Features.Orders;

internal class OrderServiceItem
{
    #region Properties
    [Key]
    public int Id { get; private set; }

    [Required]
    [StringLength(OrderServiceItemContracts.NameMaxLength)]
    public required string Name { get; set; }

    [Required]
    public required long AmountBeforeVatPerUnit { get; set; }

    [Required]
    public required int VatPercentagePerUnit { get; set; }

    [Required]
    public required int Quantity { get; set; }
    #endregion

    #region ForeignKeyProperties
    [Required]
    public int OrderId { get; set; }
    #endregion

    #region NavigationProperties
    public Order Order { get; set; } = null!;
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
