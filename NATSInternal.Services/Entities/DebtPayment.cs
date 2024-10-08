namespace NATSInternal.Services.Entities;

internal class DebtPayment : IDebtEntity<DebtPayment, DebtPaymentUpdateHistory>
{
    [Key]
    public int Id { get; set; }

    [Required]
    public long Amount { get; set; }

    [StringLength(255)]
    public string Note { get; set; }
    
    [Required]
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow.ToApplicationTime();

    [Required]
    public DateTime PaidDateTime { get; set; }

    [Required]
    public bool IsDeleted { get; set; }

    // Foreign keys.
    [Required]
    public int CustomerId { get; set; }

    [Required]
    public int CreatedUserId { get; set; }

    // Navigation properties.
    public virtual Customer Customer { get; set; }
    public virtual User CreatedUser { get; set; }
    public virtual List<DebtPaymentUpdateHistory> UpdateHistories { get; set; }

    // Properties for convinience.
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

    [NotMapped]
    public DateTime StatsDateTime
    {
        get => PaidDateTime;
        set => PaidDateTime = value;
    }

    [NotMapped]
    public static Expression<Func<DebtPayment, DateTime>> StatsDateTimeExpression
    {
        get => (debtPayment) => debtPayment.PaidDateTime;
    }
    
    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<DebtPayment> entityBuilder)
    {
        entityBuilder.HasKey(dp => dp.Id);
        entityBuilder.HasOne(dp => dp.Customer)
            .WithMany(c => c.DebtPayments)
            .HasForeignKey(dp => dp.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasOne(dp => dp.CreatedUser)
            .WithMany(u => u.DebtPayments)
            .HasForeignKey(dp => dp.CreatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasIndex(dp => dp.PaidDateTime);
        entityBuilder.HasIndex(d => d.IsDeleted);
    }
}
