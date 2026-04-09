namespace NATSInternal.Core.Entities;

[EntityTypeConfiguration(typeof(SupplyEntityConfiguration))]
[Table("supplies")]
internal class Supply
    :
        AbstractEntity<Supply>,
        IHasProductEntity<Supply, SupplyItem, SupplyUpdateHistoryData>
{
    #region Fields
    private User? _createdUser;
    #endregion

    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; protected set; } = Guid.NewGuid();

    [Column("stats_datetime")]
    [Required]
    public DateTime StatsDateTime { get; set; }

    [Column("created_datetime")]
    [Required]
    public DateTime CreatedDateTime { get; protected set; } = DateTime.UtcNow.ToApplicationTime();

    [Column("shipment_free")]
    [Required]
    public long ShipmentFee { get; set; } = 0;

    [Column("note")]
    [StringLength(HasStatsContracts.NoteMaxLength)]
    public string? Note { get; set; }

    [Column("is_deleted")]
    [Required]
    public bool IsDeleted { get; set; }
    #endregion

    #region ForeignKeyProperties
    [Column("created_user_id")]
    [Required]
    public Guid CreatedUserId { get; set; }
    #endregion

    #region CachedProperties
    [Column("cached_items_amount")]
    [Required]
    public long CachedItemsAmount { get; protected set; }
    #endregion

    #region ConcurrencyOperationTrackingProperty
    [Column("row_version")]
    [Timestamp]
    public byte[]? RowVersion { get; set; }
    #endregion

    #region NavigationProperty
    [BackingField(nameof(_createdUser))]
    public User CreatedUser
    {
        get => GetFieldOrThrowIfNull(_createdUser);
        set
        {
            CreatedUserId = value.Id;
            _createdUser = value;
        }
    }

    public List<SupplyItem> Items { get; protected set; } = new();
    public List<Photo> Photos { get; protected set; } = new();
    public List<UpdateHistory> UpdateHistories { get; protected set; } = new();
    #endregion

    #region ComputedProperties
    [NotMapped]
    public string? ThumbnailUrl => Photos
        .OrderBy(p => p.Id)
        .Select(p => p.Url)
        .FirstOrDefault();

    [NotMapped]
    public long ItemsAmount => Items.Sum(i => i.AmountPerUnit * i.Quantity);

    [NotMapped]
    public long Amount => ItemsAmount + ShipmentFee;

    [NotMapped]
    public long CachedAmount => CachedItemsAmount + ShipmentFee;

    [NotMapped]
    public DateTime? UpdatedDateTime => UpdateHistories
        .OrderByDescending(uh => uh.UpdatedDateTime)
        .Select(uh => uh.UpdatedDateTime)
        .Cast<DateTime?>()
        .FirstOrDefault();

    [NotMapped]
    public DateTime? LastUpdatedDateTime => UpdateHistories
        .OrderBy(uh => uh.UpdatedDateTime)
        .Select(uh => (DateTime?)uh.UpdatedDateTime)
        .LastOrDefault();

    [NotMapped]
    public User? LastUpdatedUser => UpdateHistories
        .OrderBy(uh => uh.UpdatedDateTime)
        .Select(uh => uh.UpdatedUser)
        .LastOrDefault();
    #endregion

    #region Methods
    public void UpdateCachedProperties()
    {
        CachedItemsAmount = Items.Sum(si => si.AmountPerUnit * si.Quantity);
    }
    #endregion
}