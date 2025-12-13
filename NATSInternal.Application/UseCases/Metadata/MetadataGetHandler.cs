using MediatR;
using NATSInternal.Application.Authorization;
using NATSInternal.Application.Localization;
using NATSInternal.Application.UseCases.Customers;
using NATSInternal.Application.UseCases.Products;
using NATSInternal.Application.UseCases.Users;
using NATSInternal.Domain.Features.Customers;
using NATSInternal.Domain.Features.Products;
using NATSInternal.Domain.Features.Users;
using System.Reflection;

namespace NATSInternal.Application.UseCases.Metadata;

internal class MetadataGetHandler : IRequestHandler<MetadataGetRequestDto, MetadataGetResponseDto>
{
    #region Fields
    private readonly IAuthorizationInternalService _authorizationService;
    private static readonly IDictionary<string, string> _displayNames;
    #endregion

    #region Constructors
    static MetadataGetHandler()
    {
        FieldInfo[] fields = typeof(DisplayNames).GetFields(BindingFlags.Public | BindingFlags.Static);
        _displayNames = fields
            .Where(f => f.GetValue(null) is not null)
            .ToDictionary(f => f.Name, f => (string)f.GetValue(null)!);
    }
    
    public MetadataGetHandler(IAuthorizationInternalService authorizationService)
    {
        _authorizationService = authorizationService;
    }
    #endregion

    #region Methods
    public async Task<MetadataGetResponseDto> Handle(
        MetadataGetRequestDto requestDto,
        CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;

        CustomerGetListRequestDto customerGetListRequestDto = new();
        ProductGetListRequestDto productGetListRequestDto = new();
        BrandGetListRequestDto brandGetListRequestDto = new();
        ProductCategoryGetListRequestDto productCategoryGetListRequestDto = new();
        UserGetListRequestDto userGetListRequestDto = new();

        MetadataGetListOptionsListResponseDto listOptionsList = new()
        {
            Customer = new()
            {
                ResourceName = nameof(Customer),
                DefaultSortByFieldName = customerGetListRequestDto.SortByFieldName,
                DefaultSortByAscending = customerGetListRequestDto.SortByAscending,
                DefaultResultsPerPage = customerGetListRequestDto.ResultsPerPage
            },
            Product = new()
            {
                ResourceName = nameof(Product),
                DefaultSortByFieldName = productGetListRequestDto.SortByFieldName,
                DefaultSortByAscending = productGetListRequestDto.SortByAscending,
                DefaultResultsPerPage = productGetListRequestDto.ResultsPerPage
            },
            Brand = new()
            {
                ResourceName = nameof(Brand),
                DefaultSortByFieldName = brandGetListRequestDto.SortByFieldName,
                DefaultSortByAscending = brandGetListRequestDto.SortByAscending,
                DefaultResultsPerPage = brandGetListRequestDto.ResultsPerPage

            },
            ProductCategory = new()
            {
                ResourceName = nameof(ProductCategory),
                DefaultSortByFieldName = productCategoryGetListRequestDto.SortByFieldName,
                DefaultSortByAscending = productCategoryGetListRequestDto.SortByAscending,
                DefaultResultsPerPage = productCategoryGetListRequestDto.ResultsPerPage

            },
            User = new()
            {
                ResourceName = nameof(User),
                DefaultSortByFieldName = userGetListRequestDto.SortByFieldName,
                DefaultSortByAscending = userGetListRequestDto.SortByAscending,
                DefaultResultsPerPage = userGetListRequestDto.ResultsPerPage
            }
        };

        return new()
        {
            DisplayNameList = _displayNames,
            ListOptionsList = listOptionsList,
            CreatingAuthorizationList = new()
            {
                CanCreateUser = _authorizationService.CanCreateUser(),
                CanCreateCustomer = _authorizationService.CanCreateCustomer(),
                CanCreateProduct = _authorizationService.CanCreateProduct(),
                CanCreateBrand = _authorizationService.CanCreateBrand(),
                CanCreateProductCategory = _authorizationService.CanCreateProductCategory()
            }
        };
    }
    #endregion
}