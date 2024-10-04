namespace NATSInternal.Services.Entities;

[Table("treatments")]
internal class Treatment : LockableEntity
{
    [Column("id")]
    [Key]
    public int Id { get; set; }

    [Column("paid_datetime")]
    [Required]
    public DateTime PaidDateTime { get; set; }

    [Column("service_amount")]
    [Required]
    public long ServiceAmount { get; set; } = 0;

    [Column("service_vat_percentage")]
    [Required]
    public int ServiceVatPercentage { get; set; } = 10;

    [Column("note")]
    [StringLength(255)]
    public string Note { get; set; }

    [Column("is_deleted")]
    [Required]
    public bool IsDeleted { get; set; } = false;

    // Foreign keys
    [Column("created_user_id")]
    [Required]
    public int CreatedUserId { get; set; }

    [Column("therapist_id")]
    [Required]
    public int TherapistId { get; set; }

    [Column("customer_id")]
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
    public long ItemProductAmount => Items.Sum(ts => ts.AmountBeforeVat);

    [NotMapped]
    public long ItemVatAmount => Items.Sum(ts => ts.VatAmount);

    [NotMapped]
    public long ServiceVatAmount => ServiceAmount / 100 * ServiceVatPercentage;

    [NotMapped]
    public long Amount => ItemProductAmount + ServiceAmount;

    [NotMapped]
    public long VatAmount => ItemVatAmount + ServiceVatAmount;

    [NotMapped]
    public long TotalAmountAfterVAT => ItemProductAmount + ItemVatAmount + ServiceAmount
        + ServiceVatAmount;

    [NotMapped]
    public DateTime? LastUpdatedDateTime => UpdateHistories
        .OrderBy(uh => uh.UpdatedDateTime)
        .Select(uh => (DateTime?)uh.UpdatedDateTime)
        .LastOrDefault();

    [NotMapped]
    public User LastUpdatedUser => UpdateHistories
        .OrderBy(uh => uh.UpdatedDateTime)
        .Select(uh => uh.User)
        .FirstOrDefault();
}