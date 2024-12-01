namespace NATSInternal.Services.Entities;

internal class DailyStats : IIdentifiableEntity<DailyStats>
{
    [Key]
    public int Id { get; set; }

    [Required]
    public long RetailGrossRevenue { get; set; }

    [Required]
    public long TreatmentGrossRevenue { get; set; }

    [Required]
    public long ConsultantGrossRevenue { get; set; }

    [Required]
    public long VatCollectedAmount { get; set; }

    [Required]
    public long DebtIncurredAmount { get; set; }

    [Required]
    public long DebtPaidAmount { get; set; }

    [Required]
    public long ShipmentCost { get; set; }

    [Required]
    public long SupplyCost { get; set; }

    [Required]
    public long UtilitiesExpenses { get; set; }

    [Required]
    public long EquipmentExpenses { get; set; }

    [Required]
    public long OfficeExpense { get; set; }

    [Required]
    public long StaffExpense { get; set; }
    
    [Required]
    public int NewCustomers { get; set; }

    [Required]
    public DateOnly RecordedDate { get; set; }

    [Required]
    public DateTime CreatedDateTime { get; set; }

    public DateTime? TemporarilyClosedDateTime { get; set; }

    public DateTime? OfficiallyClosedDateTime { get; set; }

    // Foreign key.
    [Required]
    public int MonthlyStatsId { get; set; }

    // Navigation properties.
    public virtual MonthlyStats Monthly { get; set; }

    // Properties for convinience.
    [NotMapped]
    public long Cost => SupplyCost + ShipmentCost;

    [NotMapped]
    public long Expenses => UtilitiesExpenses + EquipmentExpenses + OfficeExpense + StaffExpense;

    [NotMapped]
    public long GrossRevenue => RetailGrossRevenue + TreatmentGrossRevenue + ConsultantGrossRevenue;

    [NotMapped]
    public long NetRevenue => GrossRevenue - DebtAmount;

    [NotMapped]
    public long DebtAmount => DebtIncurredAmount - DebtPaidAmount;

    [NotMapped]
    public long GrossProfit => NetRevenue - Cost;

    [NotMapped]
    public long NetProfit => NetRevenue - (Cost + Expenses);

    [NotMapped]
    public long OperatingProfit => NetRevenue - Expenses;

    [NotMapped]
    public bool IsTemporarilyClosed => TemporarilyClosedDateTime.HasValue;

    [NotMapped]
    public bool IsOfficiallyClosed => OfficiallyClosedDateTime.HasValue;
    
    // Model configurations.
    public static void ConfigureModel(EntityTypeBuilder<DailyStats> entityBuilder)
    {
        entityBuilder.HasOne(dfs => dfs.Monthly)
            .WithMany(mfs => mfs.DailyStats)
            .HasForeignKey(dfs => dfs.MonthlyStatsId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasIndex(dfs => dfs.RecordedDate)
            .IsUnique();
    }
}
