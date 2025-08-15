namespace NATSInternal.Core.Entities;

internal class User : IHasSinglePhotoEntity<User>
{
    #region Properties
    [Key]
    public Guid Id { get; private set; }

    [Required]
    [StringLength(UserContracts.UserNameMaxLength)]
    public string UserName { get; set; } = string.Empty;

    [Required]
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow.ToApplicationTime();

    [StringLength(UserContracts.ProfilePictureUrlMaxLength)]
    public string? ProfilePictureUrl { get; set; }

    [Required]
    public bool IsDeleted { get; set; }
    #endregion

    #region ConcurrencyOperationTrackingProperty
    [Timestamp]
    public byte[]? RowVersion { get; set; }
    #endregion

    #region NavigationProperties
    public List<Role> Roles { get; private set; } = new();
    public List<Customer> CreatedCustomers { get; private set; } = new();
    public List<Supply> Supplies { get; private set; } = new();
    public List<SupplyUpdateHistory> SupplyUpdateHistories { get; private set; } = new();
    public List<Order> Orders { get; private set; } = new();
    public List<OrderUpdateHistory> OrderUpdateHistories { get; private set; } = new();
    public List<Treatment> CreatedTreatments { get; private set; } = new();
    public List<Treatment> TreatmentsInCharge { get; private set; } = new();
    public List<TreatmentUpdateHistory> TreatmentUpdateHistories { get; private set; } = new();
    public List<Consultant> Consultants { get; private set; } = new();
    public List<ConsultantUpdateHistory> ConsultantUpdateHistories { get; private set; } = new();
    public List<Expense> Expenses { get; private set; } = new();
    public List<ExpenseUpdateHistory> ExpenseUpdateHistories { get; private set; } = new();
    public List<DebtIncurrence> Debts { get; private set; } = new();
    public List<DebtIncurrenceUpdateHistory> DebtUpdateHistories { get; private set; } = new();
    public List<DebtPayment> DebtPayments { get; private set; } = new();
    public List<DebtPaymentUpdateHistory> DebtPaymentUpdateHistories { get; private set; } = new();
    public List<Announcement> CreatedAnnouncements { get; private set; } = new();
    public List<Notification> CreatedNotifications { get; private set; } = new();
    public List<Notification> ReceivedNotifications { get; private set; } = new();
    public List<Notification> ReadNotifications { get; private set; } = new();
    #endregion

    #region ComputedProperties
    [NotMapped]
    public int PowerLevel => Roles.Max(r => r.PowerLevel);

    [NotMapped]
    public List<Supply> UpdatedSupplies => SupplyUpdateHistories
        .Select(suh => suh.Supply)
        .ToList();

    [NotMapped]
    public List<Expense> UpdatedExpenses => ExpenseUpdateHistories
        .Select(euh => euh.Expense)
        .ToList();

    [NotMapped]
    public List<Order> UpdatedOrders => OrderUpdateHistories
        .Select(ouh => ouh.Order)
        .ToList();

    [NotMapped]
    public List<Treatment> UpdatedTreatments => TreatmentUpdateHistories
        .Select(tuh => tuh.Treatment)
        .ToList();

    [NotMapped]
    public List<DebtIncurrence> UpdatedDebts => DebtUpdateHistories
        .Select(duh => duh.DebtIncurrence)
        .ToList();

    [NotMapped]
    public List<DebtPayment> UpdatedDebtPayments => DebtPaymentUpdateHistories
        .Select(dpuh => dpuh.DebtPayment)
        .ToList();

    [NotMapped]
    public string? ThumbnailUrl
    {
        get => ProfilePictureUrl;
        set => ProfilePictureUrl = value;
    }
    #endregion

    #region Methods
    public bool HasPermission(string permissionName)
    {
        return Roles
            .SelectMany(r => r.Permissions)
            .Any(p => p.Name == permissionName);
    }
    #endregion

    #region StaticMethods
    public static void ConfigureModel(EntityTypeBuilder<User> entityBuilder)
    {
        entityBuilder.HasKey(u => u.Id);
        entityBuilder.HasMany(u => u.Roles)
            .WithMany(r => r.Users)
            .UsingEntity<UserRole>(
                userRole => userRole
                    .HasOne(ur => ur.Role)
                    .WithMany()
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired(),
                userRole => userRole
                    .HasOne(ur => ur.User)
                    .WithMany()
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired());
        entityBuilder.HasIndex(u => u.UserName).IsUnique();
        entityBuilder.Property(u => u.RowVersion).IsRowVersion();
        entityBuilder.HasQueryFilter(u => !u.IsDeleted);
    }
    #endregion
}