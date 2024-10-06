namespace NATSInternal.Services.Interfaces;

internal interface IDatabaseContext<TUser, TCustomer, TProduct>
    where TUser : class, IUserEntity<TUser>, new()
    where TCustomer : class, ICustomerEntity<TCustomer, TUser>, new()
    where TProduct : class, IProductEntity<TProduct>, new()

{
    DbSet<TProduct> Products { get; }
    DbSet<TCustomer> Customers { get; }
    DbSet<TUser> Users { get; }
}