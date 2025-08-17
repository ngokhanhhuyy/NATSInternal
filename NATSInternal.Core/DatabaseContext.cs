using Microsoft.EntityFrameworkCore.Metadata;

namespace NATSInternal.Core;

internal class DatabaseContext : DbContext
{
    #region StaticFields
    private static readonly JsonSerializerOptions _serializerOptions = new();
    #endregion

    #region Constructors
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    #endregion

    #region Properties
    public DbSet<Country> Countries { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductPhoto> ProductPhotos { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Supply> Supplies { get; set; }
    public DbSet<SupplyUpdateHistory> SupplyUpdateHistories { get; set; }
    public DbSet<SupplyItem> SupplyItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<OrderUpdateHistory> OrderUpdateHistories { get; set; }
    public DbSet<Treatment> Treatments { get; set; }
    public DbSet<TreatmentItem> TreatmentItems { get; set; }
    public DbSet<TreatmentPhoto> TreatmentPhotos { get; set; }
    public DbSet<TreatmentUpdateHistory> TreatmentUpdateHistories { get; set; }
    public DbSet<Consultant> Consultants { get; set; }
    public DbSet<ConsultantUpdateHistory> UpdateHistories { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<ExpensePayee> ExpensePayees { get; set; }
    public DbSet<ExpensePhoto> ExpensePhotos { get; set; }
    public DbSet<ExpenseUpdateHistory> ExpenseUpdateHistories { get; set; }
    public DbSet<Debt> DebtIncurrences { get; set; }
    public DbSet<DebtUpdateHistory> DebtIncurrenceUpdateHistories { get; set; }
    public DbSet<DebtPayment> DebtPayments { get; set; }
    public DbSet<DebtPaymentUpdateHistory> DebtPaymentUpdateHistories { get; set; }
    public DbSet<Announcement> Announcements { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NotificationReceivedUser> NotificationReceivedUsers { get; set; }
    public DbSet<NotificationReadUser> NotificationReadUsers { get; set; }
    public DbSet<DailySummary> DailyStats { get; set; }
    public DbSet<MonthlySummary> MonthlyStats { get; set; }
    #endregion

    #region Methods
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Customer.ConfigureModel(modelBuilder.Entity<Customer>());

        ProductCategory.ConfigureModel(modelBuilder.Entity<ProductCategory>());
        Product.ConfigureModel(modelBuilder.Entity<Product>());
        ProductPhoto.ConfigureModel(modelBuilder.Entity<ProductPhoto>());

        Country.ConfigureModel(modelBuilder.Entity<Country>());
        Brand.ConfigureModel(modelBuilder.Entity<Brand>());

        Supply.ConfigureModel(modelBuilder.Entity<Supply>());
        SupplyItem.ConfigureModel(modelBuilder.Entity<SupplyItem>());
        SupplyPhoto.ConfigureModel(modelBuilder.Entity<SupplyPhoto>());
        SupplyUpdateHistory.ConfigureModel(modelBuilder.Entity<SupplyUpdateHistory>());

        Order.ConfigureModel(modelBuilder.Entity<Order>());
        OrderItem.ConfigureModel(modelBuilder.Entity<OrderItem>());
        Photo.ConfigureModel(modelBuilder.Entity<Photo>());
        OrderUpdateHistory.ConfigureModel(modelBuilder.Entity<OrderUpdateHistory>());

        Treatment.ConfigureModel(modelBuilder.Entity<Treatment>());
        TreatmentItem.ConfigureModel(modelBuilder.Entity<TreatmentItem>());
        TreatmentPhoto.ConfigureModel(modelBuilder.Entity<TreatmentPhoto>());
        TreatmentUpdateHistory.ConfigureModel(modelBuilder.Entity<TreatmentUpdateHistory>());

        Consultant.ConfigureModel(modelBuilder.Entity<Consultant>());
        ConsultantUpdateHistory.ConfigureModel(modelBuilder.Entity<ConsultantUpdateHistory>());

        Expense.ConfigureModel(modelBuilder.Entity<Expense>());
        ExpensePayee.ConfigureModel(modelBuilder.Entity<ExpensePayee>());
        ExpensePhoto.ConfigureModel(modelBuilder.Entity<ExpensePhoto>());
        ExpenseUpdateHistory.ConfigureModel(modelBuilder.Entity<ExpenseUpdateHistory>());

        Debt.ConfigureModel(modelBuilder.Entity<Debt>());
        DebtUpdateHistory.ConfigureModel(
            modelBuilder.Entity<DebtUpdateHistory>(),
            _serializerOptions
        );

        DebtPayment.ConfigureModel(modelBuilder.Entity<DebtPayment>());
        DebtPaymentUpdateHistory.ConfigureModel(modelBuilder.Entity<DebtPaymentUpdateHistory>());

        Announcement.ConfigureModel(modelBuilder.Entity<Announcement>());

        Notification.ConfigureModel(modelBuilder.Entity<Notification>());
        NotificationReceivedUser.ConfigureModel(modelBuilder.Entity<NotificationReceivedUser>());
        NotificationReadUser.ConfigureModel(modelBuilder.Entity<NotificationReadUser>());

        DailyStats.ConfigureModel(modelBuilder.Entity<DailySummary>());
        MonthlyStats.ConfigureModel(modelBuilder.Entity<MonthlySummary>());

        // Identity entities
        User.ConfigureModel(modelBuilder.Entity<User>());
        Role.ConfigureModel(modelBuilder.Entity<Role>());
        UserRole.ConfigureModel(modelBuilder.Entity<UserRole>());

        modelBuilder.Entity<IdentityUserClaim<int>>(e => e.HasKey(uc => uc.Id));
        modelBuilder.Entity<IdentityUserLogin<int>>(e => e.HasKey(ul => ul.UserId));
        modelBuilder.Entity<IdentityUserToken<int>>(e => e.HasKey(ut => ut.UserId));
        modelBuilder.Entity<IdentityRoleClaim<int>>(e => e.HasKey(rc => rc.Id));

        // Set default naming convention for all models.
        foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
        {
            // Set table name.
            entity.SetTableName(entity.GetTableName().PascalCaseToSnakeCase());

            // Set columns' _names.
            foreach (IMutableProperty property in entity.GetProperties())
            {
                string name = property.Name.PascalCaseToSnakeCase();
                property.SetColumnName(name);
            }

            // Set primary keys' _names.
            foreach (IMutableKey key in entity.GetKeys())
            {
                if (key.IsPrimaryKey())
                {
                    IEnumerable<string> columnNames = key.Properties
                        .Select(p =>
                        {
                            if (p.Name.Any(char.IsUpper))
                            {
                                return p.Name.PascalCaseToSnakeCase();
                            }

                            return p.Name;
                        });
                    string name = string.Join("__", columnNames);
                    key.SetName(name);
                }
            }

            // Set foreign keys' constraint _names.
            foreach (IMutableForeignKey foreignKey in entity.GetForeignKeys())
            {
                string referencingTable = foreignKey.PrincipalEntityType
                    .GetTableName()
                    .PascalCaseToSnakeCase();
                string referencedTable = foreignKey.DeclaringEntityType
                    .GetTableName()
                    .PascalCaseToSnakeCase();
                string referencingColumns = string.Join(
                    "__",
                    foreignKey.Properties
                        .Select(p =>
                        {
                            if (p.Name.Any(char.IsUpper))
                            {
                                return p.Name.PascalCaseToSnakeCase();
                            }

                            return p.Name;
                        }));
                foreignKey.SetConstraintName(
                    $"FK__{referencingTable}__{referencedTable}__{referencingColumns}");
            }

            // Change index _names
            foreach (IMutableIndex index in entity.GetIndexes())
            {
                string indexName = index.IsUnique ? "UNIQUE__" : "INDEX__";
                indexName += index.DeclaringEntityType.GetTableName().PascalCaseToSnakeCase();
                indexName += string.Join(
                    "__",
                    index.Properties
                        .Select(p =>
                        {
                            if (p.Name.Any(char.IsUpper))
                            {
                                return p.Name.PascalCaseToSnakeCase();
                            }

                            return p.Name;
                        }));

                index.SetDatabaseName(indexName);
            }
        }
    }
    #endregion
}