namespace NATSInternal.Services.Entities;

internal class Supply
    :
        FinancialEngageableAbstractEntity,
        IHasProductEntity<Supply, SupplyItem, SupplyPhoto, SupplyUpdateHistory>
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime StatsDateTime { get; set; }

    [Required]
    public long ShipmentFee { get; set; } = 0;

    [StringLength(255)]
    public string Note { get; set; }

    [Required]
    public bool IsDeleted { get; set; }

    // Foreign keys
    [Required]
    public int CreatedUserId { get; set; }

    // Concurrency operation tracking field
    [Timestamp]
    public byte[] RowVersion { get; set; }

    // Relationships
    public virtual User CreatedUser { get; set; }
    public virtual List<SupplyItem> Items { get; set; }
    public virtual List<SupplyPhoto> Photos { get; set; }
    public virtual List<SupplyUpdateHistory> UpdateHistories { get; set; }

    // Properties for convinience.
    [NotMapped]
    public string ThumbnailUrl => Photos?
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
    public User LastUpdatedUser => UpdateHistories
        .OrderBy(uh => uh.UpdatedDateTime)
        .Select(uh => uh.UpdatedUser)
        .LastOrDefault();

    // Model configurations.
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
}