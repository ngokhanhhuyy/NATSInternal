namespace NATSInternal.Core.Entities;

[Table("expense_update_histories")]
internal class ExpenseUpdateHistory
    :
        AbstractEntity,
        IUpdateHistoryEntity<ExpenseUpdateHistory, ExpenseUpdateHistoryData>
{
    #region Fields
    private Expense? _expense;
    private User? _updatedUser;
    #endregion

    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();

    [Column("updated_datetime")]
    [Required]
    public required DateTime UpdatedDateTime { get; set; }

    [Column("reason")]
    [Required]
    [StringLength(ExpenseUpdateHistoryContracts.ReasonMaxLength)]
    public required string Reason { get; set; }

    [Required]
    [StringLength(ExpenseUpdateHistoryContracts.DataMaxLength)]
    public required ExpenseUpdateHistoryData OldData { get; set; }

    [Required]
    [StringLength(ExpenseUpdateHistoryContracts.DataMaxLength)]
    public required ExpenseUpdateHistoryData NewData { get; set; }
    #endregion

    #region ForeignKeyProperties
    [Required]
    public Guid ExpenseId { get; set; }

    [Required]
    public Guid UpdatedUserId { get; set; }
    #endregion

    #region NavigationProperties
    [BackingField(nameof(_expense))]
    public Expense Expense
    {
        get => GetFieldOrThrowIfNull(_expense);
        set
        {
            ExpenseId = value.Id;
            _expense = value;
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

    #region StaticMethods
    public static void ConfigureModel(
            EntityTypeBuilder<ExpenseUpdateHistory> entityBuilder,
            JsonSerializerOptions serializerOptions)
    {
        entityBuilder.HasKey(euh => euh.Id);
        entityBuilder
            .HasOne(euh => euh.Expense)
            .WithMany(ex => ex.UpdateHistories)
            .HasForeignKey(euh => euh.ExpenseId)
            .HasConstraintName("FK__expense_update_histories__expenses__expense_id")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        entityBuilder
            .HasOne(euh => euh.UpdatedUser)
            .WithMany(u => u.ExpenseUpdateHistories)
            .HasForeignKey(euh => euh.UpdatedUserId)
            .HasConstraintName("FK__expense_update_histories__users__updated_user_id")
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        entityBuilder
            .HasIndex(euh => euh.UpdatedDateTime)
            .HasDatabaseName("IX__expense_update_histories__updated_datetime");
        entityBuilder
            .Property(euh => euh.OldData)
            .HasColumnType("JSON")
            .HasJsonConversion(serializerOptions);
        entityBuilder
            .Property(euh => euh.NewData)
            .HasColumnType("JSON")
            .HasJsonConversion(serializerOptions);
    }
    #endregion
}