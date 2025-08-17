namespace NATSInternal.Core.Entities;

[Table("debt_update_histories")]
internal class DebtUpdateHistory
    :
        AbstractEntity<DebtUpdateHistory>,
        IUpdateHistoryEntity<DebtUpdateHistory, DebtUpdateHistoryData>
{
    #region Fields
    private Debt? _debt;
    private User? _updatedUser;
    #endregion

    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();

    [Column("updated_datetime")]
    [Required]
    public DateTime UpdatedDateTime { get; set; }

    [Column("reason")]
    [Required]
    [StringLength(255)]
    public required string Reason { get; set; }

    [Column("old_data")]
    [Required]
    [StringLength(DebtContracts.DataMaxLength)]
    public required DebtUpdateHistoryData OldData { get; set; }

    [Column("new_data")]
    [Required]
    [StringLength(DebtContracts.DataMaxLength)]
    public required DebtUpdateHistoryData NewData { get; set; }
    #endregion

    #region ForeignKeyProperties
    [Column("debt_id")]
    [Required]
    public Guid DebtId { get; set; }

    [Column("updated_user_id")]
    [Required]
    public Guid UpdatedUserId { get; set; }
    #endregion

    #region NavigationProperties
    [BackingField(nameof(_debt))]
    public Debt Debt
    {
        get => GetFieldOrThrowIfNull(_debt);
        set
        {
            DebtId = value.Id;
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

    #region StaticMethods
    public static void ConfigureModel(
            EntityTypeBuilder<DebtUpdateHistory> entityBuilder,
            JsonSerializerOptions serializerOptions)
    {
        entityBuilder.HasKey(duh => duh.Id);
        entityBuilder
            .HasOne(duh => duh.Debt)
            .WithMany(d => d.UpdateHistories)
            .HasForeignKey(duh => duh.DebtId)
            .OnDelete(DeleteBehavior.Cascade);
        entityBuilder.HasOne(duh => duh.UpdatedUser)
            .WithMany(u => u.DebtUpdateHistories)
            .HasForeignKey(duh => duh.UpdatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasIndex(duh => duh.UpdatedDateTime);
        entityBuilder
            .Property(duh => duh.OldData)
            .HasColumnType("JSON")
            .HasJsonConversion(serializerOptions);
        entityBuilder
            .Property(duh => duh.NewData)
            .HasColumnType("JSON")
            .HasJsonConversion(serializerOptions);
    }
    #endregion
}
