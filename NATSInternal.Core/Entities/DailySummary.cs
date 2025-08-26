namespace NATSInternal.Core.Entities;

[Table("daily_summaries")]
internal class DailySummary : AbstractEntity<DailySummary>
{
    #region Fields
    private MonthlySummary? _monthlySummary;
    #endregion

    #region Constructors
    protected DailySummary() { }

    public DailySummary(DateOnly recordedDate)
    {
        RecordedDay = recordedDate.Day;
        RecordedMonth = recordedDate.Month;
        RecordedYear = recordedDate.Year;
        RecordedDate = recordedDate;
    }
    #endregion

    #region Properties
    [Column("recorded_day")]
    [Key]
    public int RecordedDay { get; protected init; }

    [Column("record_month")]
    [Key]
    public int RecordedMonth { get; protected init; }

    [Column("recorded_year")]
    [Key]
    public int RecordedYear { get; protected init; }

    [Column("retail_gross_revenuue")]
    [Required]
    public long RetailGrossRevenue { get; set; }

    [Column("treatment_gross_revenue")]
    [Required]
    public long TreatmentGrossRevenue { get; set; }

    [Column("consultant_gross_revenue")]
    [Required]
    public long ConsultantGrossRevenue { get; set; }

    [Column("vat_collected_amount")]
    [Required]
    public long VatCollectedAmount { get; set; }

    [Column("debt_incurred_amount")]
    [Required]
    public long DebtIncurredAmount { get; set; }

    [Column("debt_paid_amount")]
    [Required]
    public long DebtPaidAmount { get; set; }

    [Column("shipment_cost")]
    [Required]
    public long ShipmentCost { get; set; }

    [Column("supply_cost")]
    [Required]
    public long SupplyCost { get; set; }

    [Column("utilities_expenses")]
    [Required]
    public long UtilitiesExpenses { get; set; }

    [Column("equiment_expenses")]
    [Required]
    public long EquipmentExpenses { get; set; }

    [Column("office_expense")]
    [Required]
    public long OfficeExpense { get; set; }

    [Column("staff_expense")]
    [Required]
    public long StaffExpense { get; set; }

    [Column("new_customer_count")]
    [Required]
    public int NewCustomerCount { get; set; }

    [Column("created_datetime")]
    [Required]
    public DateTime CreatedDateTime { get; set; }

    [Column("temporarily_closed_datetime")]
    public DateTime? TemporarilyClosedDateTime { get; set; }

    [Column("officially_closed_datetime")]
    public DateTime? OfficiallyClosedDateTime { get; set; }
    #endregion

    #region NavigationProperties
    [BackingField(nameof(_monthlySummary))]
    public MonthlySummary MonthlySummary
    {
        get => GetFieldOrThrowIfNull(_monthlySummary);
        init =>_monthlySummary = value;
    }
    #endregion

    #region ComputedProperties
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

    [NotMapped]
    public DateOnly RecordedDate { get; }
    #endregion
}
