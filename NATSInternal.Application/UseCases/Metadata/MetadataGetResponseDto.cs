using JetBrains.Annotations;
using CustomerGetListFieldToSort = NATSInternal.Application.UseCases.Customers.CustomerGetListRequestDto.FieldToSort;
using ProductGetListFieldToSort = NATSInternal.Application.UseCases.Products.ProductGetListRequestDto.FieldToSort;
using BrandGetListFieldToSort = NATSInternal.Application.UseCases.Products.BrandGetListRequestDto.FieldToSort;
using ProductCategoryListFieldToSort = NATSInternal.Application.UseCases.Products
    .ProductCategoryGetListRequestDto
    .FieldToSort;
using UserGetListFieldToSort = NATSInternal.Application.UseCases.Users.UserGetListRequestDto.FieldToSort;

namespace NATSInternal.Application.UseCases.Metadata;

public class MetadataGetResponseDto
{
    #region Properties
    public required IDictionary<string, string> DisplayNameList { [UsedImplicitly] get; init; }
    public required MetadataGetListOptionsListResponseDto ListOptionsList { [UsedImplicitly] get; init; }
    public required MetadataGetCreatingAuthorizationListResponseDto CreatingAuthorizationList
    {
        [UsedImplicitly] get;
        init;
    }
    #endregion
}

public class MetadataGetListOptionsListResponseDto
{
    #region Properties
    public required MetadataGetListOptionsResponseDto<CustomerGetListFieldToSort> Customer
    {
        [UsedImplicitly] get;
        init;
    }

    public required MetadataGetListOptionsResponseDto<ProductGetListFieldToSort> Product
    {
        [UsedImplicitly]get;
        init;
    }

    public required MetadataGetListOptionsResponseDto<BrandGetListFieldToSort> Brand
    {
        [UsedImplicitly] get;
        init;
    }

    public required MetadataGetListOptionsResponseDto<BrandGetListFieldToSort> ProductCategory
    {
        [UsedImplicitly] get;
        init;
    }

    public required MetadataGetListOptionsResponseDto<UserGetListFieldToSort> User
    {
        [UsedImplicitly] get;
        init;
    }
    #endregion
}

public class MetadataGetListOptionsResponseDto<TOptionEnum> where TOptionEnum : struct, Enum 
{
    #region Fields
    private static readonly IEnumerable<TOptionEnum> SortByFieldOptions;
    #endregion

    #region Constructors
    static MetadataGetListOptionsResponseDto()
    {
        SortByFieldOptions = Enum.GetValues<TOptionEnum>().ToList();
    }
    #endregion

    #region Properties
    public required string ResourceName { [UsedImplicitly] get; init; }
    public IEnumerable<string> SortByFieldNameOptions => SortByFieldOptions.Select(o => o.ToString());
    public required string? DefaultSortByFieldName { [UsedImplicitly] get; init; }
    public required bool? DefaultSortByAscending { [UsedImplicitly] get; init; }
    public required int? DefaultResultsPerPage { [UsedImplicitly] get; init; }
    #endregion
}

public class MetadataGetCreatingAuthorizationListResponseDto
{
    #region Properties
    public required bool CanCreateUser { get; init; }
    public required bool CanCreateCustomer { get; init; }
    public required bool CanCreateProduct { get; init; }
    public required bool CanCreateBrand { get; init; }
    public required bool CanCreateProductCategory { get; init; }
    #endregion
}