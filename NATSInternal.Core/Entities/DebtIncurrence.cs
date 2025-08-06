namespace NATSInternal.Core.Entities;

internal class DebtIncurrence
    :
        HasStatsAbstractEntity,
        IDebtEntity<DebtIncurrence, DebtIncurrenceUpdateHistory>
{
    [Key]
    public int Id { get; set; }

    [Required]
    public long Amount { get; set; }

    [StringLength(255)]
    public string Note { get; set; }
    
    public DateTime StatsDateTime { get; set; }

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
    public virtual List<DebtIncurrenceUpdateHistory> UpdateHistories { get; set; }

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
    public static void ConfigureModel(EntityTypeBuilder<DebtIncurrence> entityBuilder)
    {
        entityBuilder.HasKey(d => d.Id);
        entityBuilder.HasOne(d => d.Customer)
            .WithMany(c => c.DebtIncurrences)
            .HasForeignKey(d => d.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasOne(d => d.CreatedUser)
            .WithMany(u => u.Debts)
            .HasForeignKey(d => d.CreatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasIndex(d => d.StatsDateTime);
        entityBuilder.HasIndex(d => d.IsDeleted);
    }
}
