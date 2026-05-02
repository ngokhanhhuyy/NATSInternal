using Microsoft.EntityFrameworkCore;
using NATSInternal.Core.Common.Contracts;
using NATSInternal.Core.Features.Photos;
using NATSInternal.Core.Features.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NATSInternal.Core.Features.Supplies;

internal class Supply
{
    #region Properties
    [Key]
    public int Id { get; private set; }

    [Required]
    public DateTime StatsDateTime { get; set; }

    [Required]
    public long ShipmentFee { get; set; } = 0;

    [StringLength(HasStatsContracts.NoteMaxLength)]
    public string? Note { get; set; }

    [Required]
    public DateTime CreatedDateTime { get; set; }

    public DateTime? LastUpdatedDateTime { get; set; }
    public DateTime? DeletedDateTime { get; set; }
    #endregion

    #region ForeignKeyProperties
    [Required]
    public int CreatedUserId { get; set; }

    public int? LastUpdatedUserId { get; set; }

    public int? DeletedUserId { get; set; }
    #endregion

    #region CachedProperties
    [Required]
    public long CachedItemsAmount { get; private set; }
    #endregion

    #region ConcurrencyOperationTrackingProperty
    [Timestamp]
    public byte[]? RowVersion { get; set; }
    #endregion

    #region NavigationProperty
    public User CreatedUser { get; set; } = null!;
    public User? LastUpdatedUser { get; set; }
    public User? DeletedUser { get; set; }
    public List<SupplyItem> Items { get; private set; } = new();
    public List<Photo> Photos { get; private set; } = new();
    #endregion

    #region ComputedProperties
    [NotMapped]
    public Photo? Thumbnail => Photos.FirstOrDefault(p => p.IsThumbnail);

    [NotMapped]
    public long ItemsAmount => Items.Sum(i => i.AmountPerUnit * i.Quantity);

    [NotMapped]
    public long Amount => ItemsAmount + ShipmentFee;

    [NotMapped]
    public long CachedAmount => CachedItemsAmount + ShipmentFee;
    #endregion

    #region Methods
    public void ComputeCachedProperties()
    {
        CachedItemsAmount = Items.Sum(si => si.AmountPerUnit * si.Quantity);
    }
    #endregion
}