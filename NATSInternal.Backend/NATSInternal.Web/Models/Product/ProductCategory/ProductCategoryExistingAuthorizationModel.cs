using NATSInternal.Application.Authorization;

namespace NATSInternal.Web.Models;

public class ProductCategoryExistingAuthorizationModel
{
    #region Constructors
    public ProductCategoryExistingAuthorizationModel(ProductCategoryExistingAuthorizationResponseDto responseDto)
    {
        CanEdit = responseDto.CanEdit;
        CanDelete = responseDto.CanDelete;
    }
    #endregion

    #region Properties
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
    #endregion
}