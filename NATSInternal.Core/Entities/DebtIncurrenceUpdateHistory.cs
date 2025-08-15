namespace NATSInternal.Core.Entities;

internal class DebtIncurrenceUpdateHistory : IUpdateHistoryEntity<DebtIncurrenceUpdateHistory>
{
    #region Fields
    private DebtIncurrence? _debtIncurrence;
    private User? _updatedUser;
    #endregion

    #region Properties
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();

    [Required]
    public DateTime UpdatedDateTime { get; set; }

    [Required]
    [StringLength(255)]
    public required string Reason { get; set; }

    [StringLength(1000)]
    public required DebtIncurrenceUpdateHistoryData OldData { get; set; }

    [Required]
    [StringLength(1000)]
    public required DebtIncurrenceUpdateHistoryData NewData { get; set; }
    #endregion

    #region ForeignKeyProperties
    [Required]
    public Guid DebtIncurrenceId { get; set; }

    [Required]
    public Guid UpdatedUserId { get; set; }
    #endregion

    #region NavigationProperties
    public DebtIncurrence DebtIncurrence
    {
        get => _debtIncurrence ?? throw new InvalidOperationException(
            ErrorMessages.NavigationPropertyHasNotBeenLoaded.ReplacePropertyName(nameof(DebtIncurrence)));
        set => _debtIncurrence = value;
    }

    public User UpdatedUser
    {
        get => _updatedUser ?? throw new InvalidOperationException(
            ErrorMessages.NavigationPropertyHasNotBeenLoaded.ReplacePropertyName(nameof(UpdatedUser)));
        set => _updatedUser = value;
    }
    #endregion

    #region StaticMethods
    public static void ConfigureModel(EntityTypeBuilder<DebtIncurrenceUpdateHistory> builder)
    {
        builder.HasKey(duh => duh.Id);
        builder.HasOne(duh => duh.DebtIncurrence)
            .WithMany(d => d.UpdateHistories)
            .HasForeignKey(duh => duh.DebtIncurrenceId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(duh => duh.UpdatedUser)
            .WithMany(u => u.DebtUpdateHistories)
            .HasForeignKey(duh => duh.UpdatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(duh => duh.UpdatedDateTime);
        builder.Property(duh => duh.OldData)
            .HasColumnType("JSON")
            .HasConversion(
                data => JsonSerializer.Serialize(data),
                json => JsonSerializer.Deserialize<DebtIncurrenceUpdateHistoryData>(json)!
            );
        builder.Property(duh => duh.NewData).HasColumnType("JSON");
    }
    #endregion
}
