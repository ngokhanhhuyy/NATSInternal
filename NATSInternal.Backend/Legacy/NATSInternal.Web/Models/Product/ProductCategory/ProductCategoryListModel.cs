using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NATSInternal.Application.Authorization;
using NATSInternal.Application.Localization;
using NATSInternal.Application.UseCases.Products;

namespace NATSInternal.Web.Models;

public class ProductCategoryListModel : AbstractListModel<
    ProductCategoryListProductCategoryModel,
    ProductCategoryGetListRequestDto,
    ProductCategoryGetListResponseDto,
    ProductCategoryGetListProductCategoryResponseDto,
    ProductCategoryGetListRequestDto.FieldToSort>
{
    #region ProtectedMethods
    protected override void MapItemsFromResponseDtos(
        IEnumerable<ProductCategoryGetListProductCategoryResponseDto> responseDtos)
    {
        Items = responseDtos
            .Select(dto => new ProductCategoryListProductCategoryModel(dto))
            .ToList()
            .AsReadOnly();
    }
    #endregion
}

public class ProductCategoryListProductCategoryModel
{
    #region Constructors
    public ProductCategoryListProductCategoryModel(ProductCategoryGetListProductCategoryResponseDto responseDto)
    {
        Id = responseDto.Id;
        Name = responseDto.Name;
        Authorization = new(responseDto.Authorization);
    }
    #endregion

    #region Properties
    public Guid Id { get; set; }

    [DisplayName(DisplayNames.Name)]
    public string Name { get; set; }

    public ProductCategoryExistingAuthorizationModel Authorization { get; set; }
    #endregion
}