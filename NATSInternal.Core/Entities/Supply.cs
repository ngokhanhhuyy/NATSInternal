namespace NATSInternal.Core.Entities;

internal class Supply
    :
        AbstractHasStatsEntity,
        IHasProductEntity<Supply, SupplyItem, SupplyPhoto, SupplyUpdateHistory>
{
    #region Fields
    private User? _createdUser;
    #endregion

    #region Properties
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();

    [Required]
    public DateTime StatsDateTime { get; set; }

    [Required]
    public long ShipmentFee { get; set; } = 0;

    [StringLength(255)]
    public string? Note { get; set; }

    [Required]
    public bool IsDeleted { get; set; }
    #endregion

    #region ForeignKeyProperties
    [Required]
    public int CreatedUserId { get; set; }
    #endregion

    #region ConcurrencyOperationTrackingProperty
    [Timestamp]
    public byte[]? RowVersion { get; set; }
    #endregion

    #region NavigationProperty
    [BackingField(nameof(_createdUser))]
    public User CreatedUser
    {
        get => _createdUser ?? throw new InvalidOperationException(
            ErrorMessages.NavigationPropertyHasNotBeenLoaded.ReplacePropertyName(nameof(CreatedUser)));
        set => _createdUser = value;
    }

    public List<SupplyItem> Items { get; private set; } = new();
    public List<SupplyPhoto> Photos { get; private set; } = new();
    public List<SupplyUpdateHistory> UpdateHistories { get; private set; } = new();
    #endregion

    #region ComputedProperties
    [NotMapped]
    public string? ThumbnailUrl => Photos?
        .OrderBy(p => p.Id)
        .Select(p => p.Url)
        .FirstOrDefault();

    [NotMapped]
    public long ItemAmount => Items.Sum(i => i.ProductAmountPerUnit * i.Quantity);

    [NotMapped]
    public long Amount => ItemAmount + ShipmentFee;

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

    #region StaticMethods
    public static void ConfigureModel(EntityTypeBuilder<Supply> entityBuilder)
    {
        entityBuilder.HasKey(s => s.Id);
        entityBuilder.HasOne(s => s.CreatedUser)
            .WithMany(u => u.Supplies)
            .HasForeignKey(s => s.CreatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasIndex(s => s.StatsDateTime)
            .IsUnique();
        entityBuilder.HasIndex(s => s.IsDeleted);
        entityBuilder.Property(c => c.RowVersion)
            .IsRowVersion();
    }
    #endregion
}