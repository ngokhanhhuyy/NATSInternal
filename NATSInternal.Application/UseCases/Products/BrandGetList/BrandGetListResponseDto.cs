using NATSInternal.Application.Authorization;
using NATSInternal.Domain.Features.Products;

namespace NATSInternal.Application.UseCases.Products;

public class BrandGetListResponseDto : IListResponseDto<BrandGetListBrandResponseDto>
{
    #region Constructors
    public BrandGetListResponseDto(
        IEnumerable<BrandGetListBrandResponseDto> productResponseDtos,
        int pageCount,
        int itemCount)
    {
        Items = productResponseDtos;
        PageCount = pageCount;
        ItemCount = itemCount;
    }
    #endregion

    #region Properties
    public IEnumerable<BrandGetListBrandResponseDto> Items { get; }
    public int PageCount { get; }
    public int ItemCount { get; }
    #endregion
}

public class BrandGetListBrandResponseDto
{
    #region Constructors
    internal BrandGetListBrandResponseDto(Brand brand, BrandExistingAuthorizationResponseDto authorization)
    {
        Id = brand.Id;
        Name = brand.Name;
        CountryName = brand.Country?.Name;
        Authorization = authorization;
    }
    #endregion

    #region Properties
    public Guid Id { get; }
    public string Name { get; }
    public string? CountryName { get; }
    public BrandExistingAuthorizationResponseDto Authorization { get; }
    #endregion
}