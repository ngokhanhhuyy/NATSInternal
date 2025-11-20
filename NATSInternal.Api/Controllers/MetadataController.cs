using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NATSInternal.Application.Localization;
using System.Reflection;
using IAuthorizationService = NATSInternal.Application.Authorization.IAuthorizationService;
using CustomerFieldToSort = NATSInternal.Application.UseCases.Customers.CustomerGetListRequestDto.FieldToSort;
using ProductFieldToSort = NATSInternal.Application.UseCases.Products.ProductGetListRequestDto.FieldToSort;
using UserFieldToSort = NATSInternal.Application.UseCases.Users.UserGetListRequestDto.FieldToSort;

namespace NATSInternal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MetadataController : ControllerBase
{
    #region StaticFields
    private static readonly Dictionary<string, string> _displayNamesData;
    private readonly IAuthorizationService _authorizationService;
    #endregion
    
    #region Constructors
    public MetadataController(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }
    #endregion
    
    #region StaticConstructors
    static MetadataController()
    {
        FieldInfo[] fields = typeof(DisplayNames).GetFields(BindingFlags.Public | BindingFlags.Static);
        _displayNamesData = fields
            .Where(f => f.GetValue(null) is not null)
            .ToDictionary(f => f.Name, f => (string)f.GetValue(null)!);
    }
    #endregion
    
    #region Methods
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Index()
    {
        return Ok(new
        {
            DisplayNameList = _displayNamesData,
            ListOptionsList = new
            {
                User = GetOptions<UserFieldToSort>(UserFieldToSort.CreatedDateTime, true, 15),
                Customer = GetOptions<CustomerFieldToSort>(CustomerFieldToSort.FirstName, true, 15),
                Product = GetOptions<ProductFieldToSort>(ProductFieldToSort.CreatedDateTime, true, 15)
            },
            CreatingAuthorizationList = new
            {
                CanCreateUser = _authorizationService.CanCreateUser(),
                CanCreateCustomer = _authorizationService.CanCreateCustomer(),
                CanCreateProduct = _authorizationService.CanCreateProduct(),
                CanCreateBrand = _authorizationService.CanCreateBrand(),
                CanCreateProductCategory = _authorizationService.CanCreateProductCategory()
            }
        });
    }
    #endregion
    
    #region StaticMethods
    private static ListOptions<TOptionEnum> GetOptions<TOptionEnum>(
        TOptionEnum? defaultSortByFieldName = null,
        bool? defaultSortByAscending = null,
        int? defaultResultsPerPage = null) where TOptionEnum : struct, Enum
    {
        return new()
        {
            SortByFieldNameOptions = Enum.GetValues<TOptionEnum>().ToList(),
            DefaultSortByFieldName = defaultSortByFieldName,
            DefaultSortByAscending = defaultSortByAscending,
            DefaultResultsPerPage = defaultResultsPerPage
        };
    }
    #endregion
    
    #region Classes
    private class ListOptions<TOptionEnum> where TOptionEnum : struct, Enum
    {
        #region Properties
        public required List<TOptionEnum> SortByFieldNameOptions { [UsedImplicitly] get; init; }
        public required TOptionEnum? DefaultSortByFieldName { [UsedImplicitly] get; init; }
        public required bool? DefaultSortByAscending { [UsedImplicitly] get; init; }
        public required int? DefaultResultsPerPage { [UsedImplicitly] get; init; }
        #endregion
    }
    #endregion
}