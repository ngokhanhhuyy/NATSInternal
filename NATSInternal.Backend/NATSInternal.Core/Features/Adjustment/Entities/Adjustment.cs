using NATSInternal.Core.Features.Expenses;
using NATSInternal.Core.Features.Orders;
using NATSInternal.Core.Features.Payments;
using NATSInternal.Core.Features.Supplies;
using NATSInternal.Core.Features.Users;
using System.ComponentModel.DataAnnotations;

namespace NATSInternal.Core.Features.Adjustments;

internal class Adjustment
{
    #region Properties
    [Key]
    public int Id { get; private set; }

    [StringLength(AdjustmentContracts.ReasonMaxLength)]
    public string? Reason { get; set; }

    [Required]
    public required DateTime CreatedDateTime { get; set; }
    #endregion

    #region ForeignKeyProperties
    [Required]
    public required int CreatedUserId { get; set; }

    public int? ExpenseId { get; set; }

    public int? OrderId { get; set; }

    public int? PaymentId { get; set; }

    public int? SupplyId { get; set; }
    #endregion

    #region NavigationProperties
    public User CreatedUser { get; set; } = null!;
    public Expense? Expense { get; set; }
    public Order? Order { get; set; }
    public Payment? Payment { get; set; }
    public Supply? Supply { get; set; }
    #endregion
}
