namespace NATSInternal.Core.Entities;

[Table("monthly_summaries")]
internal class MonthlySummary : AbstractEntity<MonthlySummary>, IHasIdEntity<MonthlySummary>
{
    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();

    [Column("retail_gross_revenue")]
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

    [Column("equipment_expenses")]
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

    [Column("record_month")]
    [Required]
    public int RecordedMonth { get; set; }

    [Column("recorded_year")]
    [Required]
    public int RecordedYear { get; set; }

    [Column("created_datetime")]
    [Required]
    public DateTime CreatedDateTime { get; set; }

    [Column("temporarily_closed_datetime")]
    public DateTime? TemporarilyClosedDateTime { get; set; }

    [Column("officially_closed_datetime")]
    public DateTime? OfficiallyClosedDateTime { get; set; }
    #endregion

    #region NavigationProperties
    public List<DailySummary> DailyStats { get; protected set; } = new();
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
    public long NetProfit => NetRevenue - (Cost + Expenses);

    [NotMapped]
    public long GrossProfit => NetRevenue - Cost;

    [NotMapped]
    public long OperatingProfit => NetRevenue - Expenses;

    [NotMapped]
    public bool IsTemporarilyClosed => TemporarilyClosedDateTime.HasValue;

    [NotMapped]
    public bool IsOfficiallyClosed => OfficiallyClosedDateTime.HasValue;
    #endregion
}
