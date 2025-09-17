using System.Text.RegularExpressions;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NATSInternal.Domain.Features.Photos;
using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Stocks;
using NATSInternal.Domain.Features.Users;

namespace NATSInternal.Infrastructure.DbContext;

internal partial class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    #region Constructors
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    #endregion

    #region Properties
    public DbSet<User> Users { get; private set; }
    public DbSet<Role> Roles { get; private set; }
    public DbSet<Permission> Permissions { get; private set; }
    public DbSet<Country> Countries { get; private set; }
    public DbSet<Brand> Brands { get; private set; }
    public DbSet<ProductCategory> ProductCategories { get; private set; }
    public DbSet<Product> Products { get; private set; }
    public DbSet<Stock> Stocks { get; private set; }
    public DbSet<Photo> Photos { get; private set; }
    #endregion

    #region ProtectedMethods
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User entity cluster.
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new RoleEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new PermissionEntityTypeConfiguration());

        // Product entity cluster.
        modelBuilder.ApplyConfiguration(new BrandEntityTypeConfiguration());

        // Stock entity cluster.
        modelBuilder.ApplyConfiguration(new StockEntityTypeConfiguration());

        // Apply naming conventions.
        ConfigureIdentifierNames(modelBuilder);
    }
    #endregion
    
    #region PrivateMethods
    private static void ConfigureIdentifierNames(ModelBuilder modelBuilder)
    {
        // Set default naming convention for all models.
        foreach (IMutableEntityType entity in modelBuilder.Model.GetEntityTypes())
        {
            // Set table name.
            string? tableName = entity.GetTableName();
            if (tableName is not null)
            {
                entity.SetTableName(OmitAspNetPrefix(tableName).Underscore()); 
            }

            // Set columns' names.
            foreach (IMutableProperty property in entity.GetProperties())
            {
                string name = property.Name.Underscore();
                property.SetColumnName(name);
            }

            // Set primary keys' names.
            foreach (IMutableKey key in entity.GetKeys())
            {
                if (!key.IsPrimaryKey())
                {
                    continue;
                }
                
                IEnumerable<string> columnNames = key.Properties.Select(p =>
                {
                    if (p.Name.Any(char.IsUpper))
                    {
                        return ReplaceSpecialWords(p.Name.Underscore());
                    }

                    return ReplaceSpecialWords(p.Name);
                });

                string name = "PK__" + string.Join("__", columnNames);
                key.SetName(name);
            }

            // Set foreign keys' constraint _names.
            foreach (IMutableForeignKey foreignKey in entity.GetForeignKeys())
            {
                string referencingTable = OmitAspNetPrefix(foreignKey.PrincipalEntityType.GetTableName()!).Underscore();
                string referencedTable = OmitAspNetPrefix(foreignKey.DeclaringEntityType.GetTableName()!).Underscore();
                string referencingColumns = string.Join(
                    "__",
                    foreignKey.Properties.Select(p =>
                    {
                        if (p.Name.Any(char.IsUpper))
                        {
                            return ReplaceSpecialWords(p.Name.Underscore());
                        }

                        return ReplaceSpecialWords(p.Name);
                    }));
                foreignKey.SetConstraintName(
                    $"FK__{referencingTable}__{referencedTable}__{referencingColumns}");
            }

            // Change index _names
            foreach (IMutableIndex index in entity.GetIndexes())
            {
                string indexName = index.IsUnique ? "UNIQUE__" : "INDEX__";
                indexName += OmitAspNetPrefix(index.DeclaringEntityType.GetTableName()!).Underscore() + "__";
                indexName += string.Join(
                    "__",
                    index.Properties
                        .Select(p =>
                        {
                            if (p.Name.Any(char.IsUpper))
                            {
                                return ReplaceSpecialWords(p.Name.Underscore());
                            }

                            return ReplaceSpecialWords(p.Name);
                        }));

                index.SetDatabaseName(indexName);
            }
        }
    }

    private static string ReplaceSpecialWords(string name)
    {
        Dictionary<string, string> wordPairs = new()
        {
            { "user_name", "username" },
            { "date_time", "datetime" }
        };

        string resultName = name;
        foreach (KeyValuePair<string, string> pair in wordPairs)
        {
            resultName = resultName.Replace(pair.Key, pair.Value);
        }

        return resultName;
    }

    private static string OmitAspNetPrefix(string name)
    {
        return GetIdentityTableNameRegex().Replace(name, "");
    }
    #endregion
    
    #region StaticFields
    [GeneratedRegex("^(AspNet)")]
    private static partial Regex GetIdentityTableNameRegex();
    #endregion
}
