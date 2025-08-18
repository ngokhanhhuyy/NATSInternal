namespace NATSInternal.Core.Entities;

[EntityTypeConfiguration(typeof(UpdateHistoryEntityConfiguration))]
[Table("update_histories")]
internal class UpdateHistory : AbstractEntity<UpdateHistory>, IHasIdEntity<UpdateHistory>
{
    #region Fields
    private Supply? _supply;
    private Expense? _expense;
    private Order? _order;
    private Debt? _debt;
    private User? _updatedUser;
    #endregion

    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; protected set; } = Guid.NewGuid();

    [Column("updated_datetime")]
    [Required]
    public required DateTime UpdatedDateTime { get; set; } = DateTime.UtcNow.ToApplicationTime();

    [Column("reason")]
    [StringLength(UpdateHistoryContracts.ReasonMaxLength)]
    public required string Reason { get; set; }

    [Column("serialized_old_data")]
    [Required]
    [StringLength(UpdateHistoryContracts.DataMaxLength)]
    public string SerializedOldData { get; private set; } = string.Empty;

    [Column("serialized_new_data")]
    [Required]
    [StringLength(UpdateHistoryContracts.DataMaxLength)]
    public string SerializedNewData { get; private set; } = string.Empty;
    #endregion

    #region ForeignKeyProperties
    [Column("supply_id")]
    public Guid? SupplyId { get; set; }

    [Column("expense_id")]
    public Guid? ExpenseId { get; set; }

    [Column("order_id")]
    public Guid? OrderId { get; set; }

    [Column("debt_id")]
    public Guid? DebtId { get; set; }

    [Column("updated_user_id")]
    [Required]
    public Guid UpdatedUserId { get; set; }
    #endregion

    #region NavigationProperties
    [BackingField(nameof(_order))]
    public Order Order
    {
        get => GetFieldOrThrowIfNull(_order);
        set
        {
            OrderId = value?.Id;
            _order = value;
        }
    }

    [BackingField(nameof(_supply))]
    public Supply Supply
    {
        get => GetFieldOrThrowIfNull(_supply);
        set
        {
            SupplyId = value?.Id;
            _supply = value;
        }
    }

    [BackingField(nameof(_expense))]
    public Expense Expense
    {
        get => GetFieldOrThrowIfNull(_expense);
        set
        {
            ExpenseId = value?.Id;
            _expense = value;
        }
    }

    [BackingField(nameof(_debt))]
    public Debt Debt
    {
        get => GetFieldOrThrowIfNull(_debt);
        set
        {
            DebtId = value?.Id;
            _debt = value;
        }
    }

    [BackingField(nameof(_updatedUser))]
    public User UpdatedUser
    {
        get => GetFieldOrThrowIfNull(_updatedUser);
        set
        {
            UpdatedUserId = value.Id;
            _updatedUser = value;
        }
    }
    #endregion

    #region ComputedProperties
    public required object OldData { set => SerializedOldData = JsonSerializer.Serialize(value); }
    public required object NewData { set => SerializedNewData = JsonSerializer.Serialize(value); }
    #endregion

    #region Methods
    public TData GetOldData<TData>()
    {
        return JsonSerializer.Deserialize<TData>(SerializedOldData)!;
    }
    
    public TData GetNewData<TData>()
    {
        return JsonSerializer.Deserialize<TData>(SerializedNewData)!;
    }
    #endregion
}
