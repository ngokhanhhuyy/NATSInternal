using System.ComponentModel.DataAnnotations;

namespace NATSInternal.Core.Features.Orders;

internal class OrderServiceItem
{
    #region Properties
    [Key]
    public int Id { get; private set; }

    [Required]
    public required long AmountBeforeVatPerUnit { get; set; }

    [Required]
    public required long VatAmountBeforeVatPerUnit { get; set; }

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
}