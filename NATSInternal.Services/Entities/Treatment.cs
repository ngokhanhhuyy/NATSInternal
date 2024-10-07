namespace NATSInternal.Services.Entities;

internal class Treatment
    :
        LockableEntity,
        IProductExportableEntity<
            Treatment,
            TreatmentItem,
            Product,
            TreatmentPhoto,
            User,
            TreatmentUpdateHistory>
{
    [Key]
    public int Id { get; set; }

    [Required]
    public DateTime PaidDateTime { get; set; }

    [Required]
    public long ServiceAmountBeforeVat { get; set; } = 0;

    [Required]
    public int ServiceVatAmount { get; set; } = 10;

    [StringLength(255)]
    public string Note { get; set; }

    [Required]
    public bool IsDeleted { get; set; } = false;

    // Foreign keys
    [Required]
    public int CreatedUserId { get; set; }

    [Required]
    public int TherapistId { get; set; }

    [Required]
    public int CustomerId { get; set; }

    // Concurrency operation tracking field
    [Timestamp]
    public byte[] RowVersion { get; set; }

    // Navigation properties
    public virtual User CreatedUser { get; set; }
    public virtual User Therapist { get; set; }
    public virtual Customer Customer { get; set; }
    public virtual List<TreatmentItem> Items { get; set; }
    public virtual List<TreatmentPhoto> Photos { get; set; }
    public virtual List<TreatmentUpdateHistory> UpdateHistories { get; set; }

    // Properties for convinience
    [NotMapped]
    public List<TreatmentPhoto> PreTreatmentPhotos => Photos?
        .OrderBy(p => p.Id)
        .Where(p => p.Type == TreatmentPhotoType.Before)
        .ToList();

    [NotMapped]
    public List<TreatmentPhoto> PostTreatmentPhotos => Photos?
        .OrderBy(p => p.Id)
        .Where(p => p.Type == TreatmentPhotoType.After)
        .ToList();

    [NotMapped]
    public string ThumbnailUrl => Photos
        .OrderBy(p => p.Id)
        .Select(p => p.Url)
        .FirstOrDefault();

    [NotMapped]
    public long ProductAmountBeforeVat => Items.Sum(ts => ts.AmountBeforeVat);

    [NotMapped]
    public long ProductVatAmount => Items.Sum(ts => ts.VatAmount);

    [NotMapped]
    public long AmountBeforeVat => ProductAmountBeforeVat + ServiceAmountBeforeVat;

    [NotMapped]
    public long AmountAfterVat => AmountBeforeVat + ServiceVatAmount;

    [NotMapped]
    public long VatAmount => ProductVatAmount + ServiceVatAmount;

    [NotMapped]
    public DateTime? LastUpdatedDateTime => UpdateHistories
        .OrderBy(uh => uh.UpdatedDateTime)
        .Select(uh => (DateTime?)uh.UpdatedDateTime)
        .LastOrDefault();

    [NotMapped]
    public User LastUpdatedUser => UpdateHistories
        .OrderBy(uh => uh.UpdatedDateTime)
        .Select(uh => uh.UpdatedUser)
        .FirstOrDefault();

    [NotMapped]
    public DateTime StatsDateTime
    {
        get => PaidDateTime;
        set => PaidDateTime = value;
    }

    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<Treatment> entityBuilder)
    {
        entityBuilder.HasKey(t => t.Id);
        entityBuilder.HasOne(t => t.CreatedUser)
            .WithMany(u => u.CreatedTreatments)
            .HasForeignKey(t => t.CreatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasOne(t => t.Therapist)
            .WithMany(u => u.TreatmentsInCharge)
            .HasForeignKey(t => t.TherapistId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasOne(t => t.Customer)
            .WithMany(c => c.Treatments)
            .HasForeignKey(t => t.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasIndex(t => t.PaidDateTime);
        entityBuilder.HasIndex(t => t.IsDeleted);
        entityBuilder.Property(t => t.RowVersion)
            .IsRowVersion();
    }
}