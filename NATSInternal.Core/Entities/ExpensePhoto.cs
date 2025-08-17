namespace NATSInternal.Core.Entities;

[Table("expense_photos")]
internal class ExpensePhoto : AbstractEntity<ExpensePhoto>, IPhotoEntity<ExpensePhoto>
{
    #region Fields
    private Expense? _expense;
    #endregion

    #region Properties
    [Column("id")]
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();

    [Column("url")]
    [Required]
    [StringLength(PhotoContracts.UrlMaxLength)]
    public required string Url { get; set; }
    #endregion

    #region ForeignKeyProperties
    [Column("expense_id")]
    [Required]
    public Guid ExpenseId { get; set; }
    #endregion

    #region ConcurrencyOperationTrackingProperties
    [Column("row_version")]
    [Timestamp]
    public byte[]? RowVersion { get; set; }
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
    #endregion

    #region StaticMethods
    public static void ConfigureModel(EntityTypeBuilder<ExpensePhoto> entityBuilder)
    {
        entityBuilder.HasKey(ep => ep.Id);
        entityBuilder
            .HasOne(p => p.Expense)
            .WithMany(ex => ex.Photos)
            .HasForeignKey(ex => ex.ExpenseId)
            .HasConstraintName("FK__expense_photos__expenses__expense_id")
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();
        entityBuilder
            .HasIndex(p => p.Url)
            .HasDatabaseName("IX__expense_photos__url")
            .IsUnique();
        entityBuilder
            .Property(c => c.RowVersion)
            .IsRowVersion();
    }
    #endregion
}