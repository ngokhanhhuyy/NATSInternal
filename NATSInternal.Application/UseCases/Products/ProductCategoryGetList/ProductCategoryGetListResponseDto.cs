using NATSInternal.Application.Authorization;
using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.UseCases.Products;

public class ProductCategoryGetListResponseDto
    : IListResponseDto<ProductCategoryGetListProductCategoryResponseDto>
{
    #region Constructors
    public ProductCategoryGetListResponseDto(
        IEnumerable<ProductCategoryGetListProductCategoryResponseDto> productResponseDtos,
        int pageCount,
        int itemCount,
        bool canCreate)
    {
        Items = productResponseDtos;
        PageCount = pageCount;
        ItemCount = itemCount;
        CanCreate = canCreate;
    }
    #endregion

    #region Properties
    public IEnumerable<ProductCategoryGetListProductCategoryResponseDto> Items { get; }
    public int PageCount { get; }
    public int ItemCount { get; }
    public bool CanCreate { get; }
    #endregion
}

public class ProductCategoryGetListProductCategoryResponseDto
{
    #region Constructors
    internal ProductCategoryGetListProductCategoryResponseDto(
        ProductCategory category,
        ProductCategoryExistingAuthorizationResponseDto authorization)
    {
        Id = category.Id;
        Name = category.Name;
        Authorization = authorization;
    }
    #endregion

    #region Properties
    public Guid Id { get; }
    public string Name { get; }
    public ProductCategoryExistingAuthorizationResponseDto Authorization { get; }
    #endregion
}