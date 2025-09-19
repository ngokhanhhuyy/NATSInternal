using Microsoft.EntityFrameworkCore;
using NATSInternal.Domain.Features.Products;
using NATSInternal.Infrastructure.DbContext;

namespace NATSInternal.Infrastructure.Repositories;

internal class ProductRepository : IProductRepository
{
    #region Fields
    private readonly AppDbContext _context;
    #endregion

    #region Constructors
    public ProductRepository(AppDbContext context)
    {
        _context = context;
    }
    #endregion

    #region Methods
    public async Task<Product?> GetProductIncludingBrandWithCountryAndCategoryByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        return await _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public void AddProduct(Product product)
    {
        _context.Products.Add(product);
    }

    public void UpdateProduct(Product product)
    {
        _context.Products.Update(product);
    }

    public async Task<Brand?> GetBrandIncludingCountryByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Brands
            .Include(b => b.Country)
            .SingleOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    public void AddBrand(Brand brand)
    {
        _context.Brands.Add(brand);
    }

    public void UpdateBrand(Brand brand)
    {
        _context.Brands.Update(brand);
    }

    public void RemoveBrand(Brand brand)
    {
        _context.Brands.Remove(brand);
    }
    #endregion
}
