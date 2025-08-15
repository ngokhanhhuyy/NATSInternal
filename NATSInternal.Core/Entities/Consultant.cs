namespace NATSInternal.Core.Entities;

internal class Consultant
    :
        HasStatsAbstractEntity,
        IRevenueEntity<Consultant, ConsultantUpdateHistory>
{
    #region Fields
    private Customer? _customer;
    private User? _createdUser;
    #endregion

    #region Properties
    [Key]
    public Guid Id { get; private set; } = Guid.NewGuid();

    [Required]
    public DateTime StatsDateTime { get; set; }

    [Required]
    public long AmountBeforeVat { get; set; }

    [Required]
    public long VatAmount { get; set; }

    [StringLength(ConsultantContracts.NoteMaxLength)]
    public string? Note { get; set; }

    [Required]
    public bool IsDeleted { get; set; }
    #endregion

    #region ForeignKeyProperties
    [Required]
    public required Guid CustomerId { get; set; }

    [Required]
    public int CreatedUserId { get; set; }
    #endregion

    #region NavigationProperties
    [BackingField(nameof(Customer))]
    public Customer Customer
    {
        get => _customer ?? throw new InvalidOperationException(
            ErrorMessages.NavigationPropertyHasNotBeenLoaded.ReplacePropertyName(nameof(Customer)));
        set => _customer = value;
    }

    public User CreatedUser
    {
        get => _createdUser ?? throw new InvalidOperationException(
            ErrorMessages.NavigationPropertyHasNotBeenLoaded.ReplacePropertyName(nameof(CreatedUser)));
        set => _createdUser = value;
    }
    
    public List<ConsultantUpdateHistory> UpdateHistories { get; private set; } = new();
    #endregion

    #region ComputedProperties
    [NotMapped]
    public DateTime? LastUpdatedDateTime => UpdateHistories
        .OrderBy(uh => uh.UpdatedDateTime)
        .Select(uh => (DateTime?)uh.UpdatedDateTime)
        .LastOrDefault();

    [NotMapped]
    public User? LastUpdatedUser => UpdateHistories
        .OrderBy(uh => uh.UpdatedDateTime)
        .Select(uh => uh.UpdatedUser)
        .LastOrDefault();
    #endregion

    #region StaticMethods
    public static void ConfigureModel(EntityTypeBuilder<Consultant> entityBuilder)
    {
        entityBuilder.HasKey(c => c.Id);
        entityBuilder.HasOne(cst => cst.Customer)
            .WithMany(ctm => ctm.Consultants)
            .HasForeignKey(cst => cst.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasOne(cst => cst.CreatedUser)
            .WithMany(u => u.Consultants)
            .HasForeignKey(cst => cst.CreatedUserId)
            .OnDelete(DeleteBehavior.Restrict);
        entityBuilder.HasIndex(cst => cst.StatsDateTime);
        entityBuilder.HasIndex(cst => cst.IsDeleted);
    }
    #endregion
}