namespace NATSInternal.Core.Entities;

[Table("consultant_update_histories")]
internal class ConsultantUpdateHistory
    :
        AbstractEntity<ConsultantUpdateHistory>,
        IUpdateHistoryEntity<ConsultantUpdateHistory, ConsultantUpdateHistoryData>
{
    #region Fields
    private Consultant? _consultant;
    private User? _updatedUser;
    #endregion

    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column("updated_datetime")]
    [Required]
    public required DateTime UpdatedDateTime { get; set; }

    [Column("reason")]
    [StringLength(255)]
    public required string Reason { get; set; }

    [Column("old_data")]
    [Required]
    [StringLength(2000)]
    public required ConsultantUpdateHistoryData OldData { get; set; }

    [Column("new_data")]
    [Required]
    [StringLength(2000)]
    public required ConsultantUpdateHistoryData NewData { get; set; }
    #endregion

    #region ForeignKeyProperties
    [Column("consultant_id")]
    [Required]
    public Guid ConsultantId { get; set; }

    [Column("updated_user_id")]
    [Required]
    public Guid UpdatedUserId { get; set; }
    #endregion

    #region NavigationProperties
    [BackingField(nameof(_consultant))]
    public Consultant Consultant
    {
        get => GetFieldOrThrowIfNull(_consultant);
        set
        {
            ConsultantId = value.Id;
            _consultant = value;
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
            EntityTypeBuilder<ConsultantUpdateHistory> entityBuilder,
            JsonSerializerOptions serializerOptions)
    {
        entityBuilder.HasKey(cuh => cuh.Id);
        entityBuilder
            .HasOne(cuh => cuh.Consultant)
            .WithMany(c => c.UpdateHistories)
            .HasForeignKey(cuh => cuh.ConsultantId)
            .HasConstraintName("FK__consultant_update_histories__consultants__consultants")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        entityBuilder
            .HasOne(cuh => cuh.UpdatedUser)
            .WithMany(u => u.ConsultantUpdateHistories)
            .HasForeignKey(cuh => cuh.UpdatedUserId)
            .HasConstraintName("FK__consultant_update_histories__users__updated_user_id")
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
        entityBuilder
            .HasIndex(cuh => cuh.UpdatedDateTime)
            .HasDatabaseName("IX__consulant_update_histories__updated_datetime");
        entityBuilder
            .Property(cuh => cuh.OldData)
            .HasColumnType("JSON")
            .HasJsonConversion(serializerOptions);
        entityBuilder
            .Property(cuh => cuh.NewData)
            .HasColumnType("JSON")
            .HasJsonConversion(serializerOptions);
    }
    #endregion
}