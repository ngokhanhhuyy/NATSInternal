namespace NATSInternal.Core.Entities;

internal class ExpensePayee : IIdentifiableEntity<ExpensePayee>
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    // Concurrency operation tracking field
    [Timestamp]
    public byte[] RowVersion { get; set; }

    // Navigation properties
    public virtual List<Expense> Expenses { get; set; }

    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<ExpensePayee> entityBuilder)
    {
        entityBuilder.HasIndex(ep => ep.Name)
            .IsUnique();
        entityBuilder.Property(c => c.RowVersion)
            .IsRowVersion();
    }
}