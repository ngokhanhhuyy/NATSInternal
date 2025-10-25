using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NATSInternal.Application.Localization;
using NATSInternal.Application.UseCases.Products;
using NATSInternal.Application.UseCases.Users;
using System.Reflection;
using IAuthorizationService = NATSInternal.Application.Authorization.IAuthorizationService;
using UserFieldToSort = NATSInternal.Application.UseCases.Users.UserGetListRequestDto.FieldToSort;
using ProductFieldToSort = NATSInternal.Application.UseCases.Products.ProductGetListRequestDto.FieldToSort;

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
            FieldToSortOptionsList = new
            {
                User = GetFieldToSortOptions<UserFieldToSort>(UserFieldToSort.CreatedDateTime),
                Product = GetFieldToSortOptions<ProductFieldToSort>(ProductFieldToSort.CreatedDateTime)
            },
            CreatingAuthorizationList = new
            {
                CanCreateUser = _authorizationService.CanCreateUser(),
                CanCreateProduct = _authorizationService.CanCreateProduct(),
                CanCreateBrand = _authorizationService.CanCreateBrand(),
                CanCreateProductCategory = _authorizationService.CanCreateProductCategory()
            }
        });
    }
    #endregion
    
    #region StaticMethods
    private static FieldToSortOptions GetFieldToSortOptions<TEnum>(TEnum? defaultOption = null)
        where TEnum : struct, Enum
    {
        return new()
        {
            Options = Enum.GetNames<TEnum>().ToList(),
            DefaultOption = defaultOption?.ToString()
        };
    }
    #endregion
    
    #region Classes
    private class FieldToSortOptions
    {
        #region Properties
        public required List<string> Options { [UsedImplicitly] get; init; }
        public required string? DefaultOption { [UsedImplicitly] get; init; }
        #endregion
    }
    #endregion
}