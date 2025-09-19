namespace NATSInternal.Domain.Features.Products;

internal interface IProductRepository
{
    #region Methods
    Task<Product?> GetProductIncludingBrandWithCountryAndCategoryByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    void AddProduct(Product product);

    void UpdateProduct(Product product);

    Task<Brand?> GetBrandIncludingCountryByIdAsync(Guid id, CancellationToken cancellationToken = default);

    void AddBrand(Brand brand);

    void UpdateBrand(Brand brand);

    void RemoveBrand(Brand brand);
    #endregion
}