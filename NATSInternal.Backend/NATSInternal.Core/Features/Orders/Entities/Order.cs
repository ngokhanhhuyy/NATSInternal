using NATSInternal.Core.Common.Contracts;
using NATSInternal.Core.Features.Customers;
using NATSInternal.Core.Features.Photos;
using NATSInternal.Core.Features.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace NATSInternal.Core.Features.Orders;

internal class Order
{
    #region Properties
    [Key]
    public int Id { get; private set; }

    [Required]
    public required OrderType Type { get; set; }

    [Required]
    public required DateTime StatsDateTime { get; set; }

    [StringLength(HasStatsContracts.NoteMaxLength)]
    public string? Note { get; set; }

    [Required]
    public required DateTime CreatedDateTime { get; set; }

    public DateTime? LastUpdatedDateTime { get; set; }

    public DateTime? DeletedDateTime { get; set; }
    #endregion

    #region ForeignKeyProperties
    [Required]
    public required int CustomerId { get; set; }

    [Required]
    public required int CreatedUserId { get; set; }

    public int? LastUpdatedUserId { get; set; }

    public int? DeletedUserId { get; set; }
    #endregion

    #region CachedProperties
    [Required]
    public long CachedProductItemsAmountBeforeVat { get; protected set; }

    [Required]
    public long CachedProductItemsVatAmount { get; protected set; }

    [Required]
    public long CachedServiceItemsAmountBeforeVat { get; protected set; }

    [Required]
    public long CachedServiceItemsVatAmount { get; protected set; }
    #endregion

    #region NavigationProperties
    public Customer Customer { get; set; } = null!;
    public User CreatedUser { get; set; } = null!;
    public User? LastUpdatedUser { get; set; } = null!;
    public User? DeletedUser { get; set; } = null!;
    public List<OrderProductItem> ProductItems { get; protected set; } = new();
    public List<OrderServiceItem> ServiceItems { get; protected set; } = new();
    public List<Photo> Photos { get; protected set; } = new();
    #endregion

    #region ComputedProperties
    [NotMapped]
    public string? ThumbnailUrl => Photos
        .OrderBy(p => p.Id)
        .Select(p => p.Url)
        .FirstOrDefault();

    [NotMapped]
    public long ProductAmountBeforeVat => ProductItems.Sum(i => i.AmountBeforeVatPerUnit * i.Quantity);

    [NotMapped]
    public long ProductVatAmount => ProductItems.Sum(i => i.VatAmountPerUnit * i.Quantity);

    [NotMapped]
    public long AmountBeforeVat => ProductAmountBeforeVat;

    [NotMapped]
    public long AmountAfterVat => ProductAmountBeforeVat + ProductVatAmount;

    [NotMapped]
    public long VatAmount => ProductVatAmount;

    [NotMapped]
    public long CachedAmountAfterVat
    {
        get
        {
            long productAmount = CachedProductItemsAmountBeforeVat + CachedProductItemsVatAmount;
            long serviceAmount = CachedServiceItemsAmountBeforeVat + CachedServiceItemsVatAmount;

            return productAmount + serviceAmount;
        }
    }

    [NotMapped]
    public static Expression<Func<Order, long>> AmountAfterVatExpression => (order) =>
        order.ProductItems.Sum(oi => (oi.AmountBeforeVatPerUnit + oi.VatAmountPerUnit) * oi.Quantity);
    #endregion

    #region Methods
    public void ComputeCachedProperties()
    {
        CachedProductItemsAmountBeforeVat = 0;
        CachedProductItemsVatAmount = 0;
        foreach (OrderProductItem productItem in ProductItems)
        {
            CachedProductItemsAmountBeforeVat += productItem.AmountBeforeVatPerUnit * productItem.Quantity;
            CachedProductItemsVatAmount += productItem.VatAmountPerUnit * productItem.Quantity;
        }
        
        CachedServiceItemsAmountBeforeVat = 0;
        CachedServiceItemsVatAmount = 0;
        foreach (OrderServiceItem serviceItem in ServiceItems)
        {
            CachedServiceItemsAmountBeforeVat += serviceItem.AmountBeforeVatPerUnit * serviceItem.Quantity;
            CachedServiceItemsVatAmount += serviceItem.AmountBeforeVatPerUnit * serviceItem.Quantity;
        }
    }
    #endregion
}