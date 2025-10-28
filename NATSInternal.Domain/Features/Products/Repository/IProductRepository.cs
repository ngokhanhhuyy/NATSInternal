namespace NATSInternal.Domain.Features.Products;

internal interface IProductRepository
{
    #region Methods
    Task<Product?> GetProductByIdIncludingBrandWithCountryAndCategoryAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<int> GetProductCountByCategoryIdAsync(Guid categoryId, CancellationToken cancellationToken = default);

    void AddProduct(Product product);

    void UpdateProduct(Product product);

    Task<Brand?> GetBrandByIdIncludingCountryAsync(Guid id, CancellationToken cancellationToken = default);

    void AddBrand(Brand brand);

    void UpdateBrand(Brand brand);

    void RemoveBrand(Brand brand);

    Task<ProductCategory?> GetCategoryByNameAsync(string name, CancellationToken cancellationToken = default);

    void AddCategory(ProductCategory category);

    void UpdateCategory(ProductCategory category);

    void RemoveCategory(ProductCategory category);
    #endregion
}