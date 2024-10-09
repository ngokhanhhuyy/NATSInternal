namespace NATSInternal.Services.Entities;

internal class Consultant
    :
        FinancialEngageableAbstractEntity,
        IRevenueEntity<Consultant, ConsultantUpdateHistory>
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public DateTime PaidDateTime { get; set; }

    [Required]
    public long AmountBeforeVat { get; set; }

    [Required]
    public long VatAmount { get; set; }

    [StringLength(255)]
    public string Note { get; set; }

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
    public virtual List<ConsultantUpdateHistory> UpdateHistories { get; set; }

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
    public static Expression<Func<Consultant, DateTime>> StatsDateTimeExpression
    {
        get => (consultant) => consultant.PaidDateTime;
    }

    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<Consultant> entityBuilder)
    {
        entityBuilder.HasKey(c => c.Id);
        entityBuilder.HasOne(cst => cst.Customer)
            .WithMany(ctm => ctm.Consultants)
            .HasForeignKey(cst => cst.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasOne(cst => cst.CreatedUser)
            .WithMany(u => u.Consultants)
            .HasForeignKey(cst => cst.CreatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasIndex(cst => cst.IsDeleted);
    }
}