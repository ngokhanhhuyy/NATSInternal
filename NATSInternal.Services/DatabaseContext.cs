using Microsoft.EntityFrameworkCore.Metadata;

namespace NATSInternal.Services;

internal class DatabaseContext
    :
        IdentityDbContext<
            User,
            Role,
            int,
            IdentityUserClaim<int>,
            UserRole,
            IdentityUserLogin<int>,
            IdentityRoleClaim<int>,
            IdentityUserToken<int>>
        
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

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
    public DbSet<DebtIncurrence> DebtIncurrences { get; set; }
    public DbSet<DebtIncurrenceUpdateHistory> DebtIncurrenceUpdateHistories { get; set; }
    public DbSet<DebtPayment> DebtPayments { get; set; }
    public DbSet<DebtPaymentUpdateHistory> DebtPaymentUpdateHistories { get; set; }
    public DbSet<Announcement> Announcements { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<NotificationReceivedUser> NotificationReceivedUsers { get; set; }
    public DbSet<NotificationReadUser> NotificationReadUsers { get; set; }
    public DbSet<DailyStats> DailyStats { get; set; }
    public DbSet<MonthlyStats> MonthlyStats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ConfigureEntity<Customer>();

        modelBuilder.ConfigureEntity<ProductCategory>();
        modelBuilder.ConfigureEntity<Product>();
        modelBuilder.ConfigureEntity<ProductPhoto>();

        modelBuilder.ConfigureEntity<Country>();
        modelBuilder.ConfigureEntity<Brand>();

        modelBuilder.ConfigureEntity<Supply>();
        modelBuilder.ConfigureEntity<SupplyItem>();
        modelBuilder.ConfigureEntity<SupplyPhoto>();
        modelBuilder.ConfigureEntity<SupplyUpdateHistory>();

        modelBuilder.ConfigureEntity<Order>();
        modelBuilder.ConfigureEntity<OrderItem>();
        modelBuilder.ConfigureEntity<OrderPhoto>();
        modelBuilder.ConfigureEntity<OrderUpdateHistory>();

        modelBuilder.ConfigureEntity<Treatment>();
        modelBuilder.ConfigureEntity<TreatmentItem>();
        modelBuilder.ConfigureEntity<TreatmentPhoto>();
        modelBuilder.ConfigureEntity<TreatmentUpdateHistory>();

        modelBuilder.ConfigureEntity<Consultant>();
        modelBuilder.ConfigureEntity<ConsultantUpdateHistory>();
        
        modelBuilder.ConfigureEntity<Expense>();
        modelBuilder.ConfigureEntity<ExpensePayee>();
        modelBuilder.ConfigureEntity<ExpensePhoto>();
        modelBuilder.ConfigureEntity<ExpenseUpdateHistory>();
        
        modelBuilder.ConfigureEntity<DebtIncurrence>();
        modelBuilder.ConfigureEntity<DebtIncurrenceUpdateHistory>();
        
        modelBuilder.ConfigureEntity<DebtPayment>();
        modelBuilder.ConfigureEntity<DebtPaymentUpdateHistory>();
        
        modelBuilder.ConfigureEntity<Announcement>();
        
        modelBuilder.ConfigureEntity<Notification>();
        modelBuilder.ConfigureEntity<NotificationReceivedUser>();
        modelBuilder.ConfigureEntity<NotificationReadUser>();

        modelBuilder.ConfigureEntity<DailyStats>();
        modelBuilder.ConfigureEntity<MonthlyStats>();

        // Identity entities
        modelBuilder.ConfigureEntity<User>();
        modelBuilder.ConfigureEntity<Role>();
        modelBuilder.ConfigureEntity<UserRole>();

        modelBuilder.Entity<IdentityUserClaim<int>>(e => e.HasKey(uc => uc.Id));
        modelBuilder.Entity<IdentityUserLogin<int>>(e => e.HasKey(ul => ul.UserId));
        modelBuilder.Entity<IdentityUserToken<int>>(e => e.HasKey(ut => ut.UserId));
        modelBuilder.Entity<IdentityRoleClaim<int>>(e => e.HasKey(rc => rc.Id));

        // Set default naming convention for all models.
        foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
        {
            // Set table name.
            entity.SetTableName(entity.GetTableName().PascalCaseToSnakeCase());

            // Set columns' names.
            foreach (IMutableProperty property in entity.GetProperties())
            {
                string name = property.Name
                    .PascalCaseToSnakeCase()
                    .Replace("full_name", "fullname")
                    .Replace("user_user", "username");
                property.SetColumnName(name);
            }

            // Set primary keys' names.
            foreach (IMutableKey key in entity.GetKeys())
            {
                if (key.IsPrimaryKey())
                {
                    IEnumerable<string> columnNames = key.Properties
                        .Select(p =>
                        {
                            if (p.Name.Any(char.IsUpper))
                            {
                                return p.Name
                                    .PascalCaseToSnakeCase()
                                    .Replace("full_name", "fullname")
                                    .Replace("user_user", "username");
                            }

                            return p.Name;
                        });
                    string name = string.Join("__", columnNames);
                    key.SetName(name);
                }
            }

            // Set foreign keys' constraint names.
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
                                return p.Name
                                    .PascalCaseToSnakeCase()
                                    .Replace("full_name", "fullname")
                                    .Replace("user_user", "username");
                            }

                            return p.Name;
                        }));
                foreignKey.SetConstraintName(
                    $"FK__{referencingTable}__{referencedTable}__{referencingColumns}");
            }

            // Change index names
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
                                return p.Name
                                    .PascalCaseToSnakeCase()
                                    .Replace("full_name", "fullname")
                                    .Replace("user_user", "username");
                            }

                            return p.Name;
                        }));

                index.SetDatabaseName(indexName);
            }
        }
    }
}