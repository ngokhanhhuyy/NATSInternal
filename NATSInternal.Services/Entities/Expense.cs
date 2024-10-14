namespace NATSInternal.Services.Entities;

internal class Expense
    :
        FinancialEngageableAbstractEntity,
        ICostEntity<Expense, ExpenseUpdateHistory>,
        IHasPhotoEntity<Expense, ExpensePhoto>
{
    [Key]
    public int Id { get; set; }

    [Required]
    public long Amount { get; set; }

    [Required]
    public DateTime StatsDateTime { get; set; }
    
    [Required]
    public ExpenseCategory Category { get; set; }

    [StringLength(255)]
    public string Note { get; set; }

    [Required]
    public bool IsDeleted { get; set; }

    // Foreign keys
    [Required]
    public int CreatedUserId { get; set; }

    [Required]
    public int PayeeId { get; set; }

    // Concurrency operation tracking field.
    [Timestamp]
    public byte[] RowVersion { get; set; }

    // Navigation properties
    public virtual User CreatedUser { get; set; }
    public virtual ExpensePayee Payee { get; set; }
    public virtual List<ExpensePhoto> Photos { get; set; }
    public virtual List<ExpenseUpdateHistory> UpdateHistories { get; set; }

    // Properties for convinience.
    [NotMapped]
    public string ThumbnailUrl => Photos
        .OrderBy(p => p.Id)
        .Select(p => p.Url)
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
    public static void ConfigureModel(EntityTypeBuilder<Expense> entityBuilder)
    {
        entityBuilder.HasKey(ex => ex.Id);
        entityBuilder.HasOne(ex => ex.CreatedUser)
            .WithMany(u => u.Expenses)
            .HasForeignKey(ex => ex.CreatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasOne(ex => ex.Payee)
            .WithMany(exp => exp.Expenses)
            .HasForeignKey(exp => exp.PayeeId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.Property(c => c.RowVersion)
            .IsRowVersion();
    }
}