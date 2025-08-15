namespace NATSInternal.Core.Entities;

internal class ConsultantUpdateHistory : IUpdateHistoryEntity<ConsultantUpdateHistory>
{
    #region Fields
    private Consultant? _consultant;
    private User? _updatedUser;
    #endregion

    #region Properties
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public DateTime UpdatedDateTime { get; set; }

    [StringLength(255)]
    public required string Reason { get; set; }

    [StringLength(1000)]
    public required string OldData { get; set; }

    [Required]
    [StringLength(1000)]
    public required string NewData { get; set; }

    // Foreign keys
    [Required]
    public Guid ConsultantId { get; set; }

    [Required]
    public Guid UpdatedUserId { get; set; }
    #endregion

    #region NavigationProperties
    [BackingField(nameof(_consultant))]
    public Consultant Consultant
    {
        get => _consultant ?? throw new InvalidOperationException(
            ErrorMessages.NavigationPropertyHasNotBeenLoaded.ReplacePropertyName(nameof(Consultant)));
        set => _consultant = value;
    }

    public User UpdatedUser
    {
        get => _updatedUser ?? throw new InvalidOperationException(
            ErrorMessages.NavigationPropertyHasNotBeenLoaded.ReplacePropertyName(nameof(UpdatedUser)));
        set => _updatedUser = value;
    }
    #endregion

    #region StaticMethods
    public static void ConfigureModel(EntityTypeBuilder<ConsultantUpdateHistory> entityBuilder)
    {
        entityBuilder.HasKey(cuh => cuh.Id);
        entityBuilder.HasOne(cuh => cuh.Consultant)
            .WithMany(c => c.UpdateHistories)
            .HasForeignKey(cuh => cuh.ConsultantId)
            .OnDelete(DeleteBehavior.Cascade);
        entityBuilder.HasOne(cuh => cuh.UpdatedUser)
            .WithMany(u => u.ConsultantUpdateHistories)
            .HasForeignKey(cuh => cuh.UpdatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasIndex(cuh => cuh.UpdatedDateTime);
        entityBuilder.Property(cuh => cuh.OldData).HasColumnType("JSON");
        entityBuilder.Property(cuh => cuh.NewData).HasColumnType("JSON");
    }
    #endregion
}