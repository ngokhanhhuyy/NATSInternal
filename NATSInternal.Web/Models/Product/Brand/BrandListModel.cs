using System.ComponentModel;
using NATSInternal.Application.Localization;
using NATSInternal.Application.UseCases.Products;

namespace NATSInternal.Web.Models;

public class BrandListModel : AbstractListModel<
    BrandListBrandModel,
    BrandGetListRequestDto,
    BrandGetListResponseDto,
    BrandGetListBrandResponseDto,
    BrandGetListRequestDto.FieldToSort>
{
    #region ProtectedMethods
    protected override void MapItemsFromResponseDtos(IEnumerable<BrandGetListBrandResponseDto> responseDtos)
    {
        Items = responseDtos
            .Select(dto => new BrandListBrandModel(dto))
            .ToList()
            .AsReadOnly();
    }
    #endregion
}

public class BrandListBrandModel
{
    #region Constructors
    public BrandListBrandModel(BrandGetListBrandResponseDto responseDto)
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

    public BrandExistingAuthoziationModel Authorization { get; set; }
    #endregion
}