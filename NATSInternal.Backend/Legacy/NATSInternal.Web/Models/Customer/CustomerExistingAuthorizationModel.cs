using NATSInternal.Application.Authorization;

namespace NATSInternal.Web.Models;

public class CustomerExistingAuthorizationModel
{
    #region Constructors
    public CustomerExistingAuthorizationModel(CustomerExistingAuthorizationResponseDto responseDto)
    {
        CanEdit = responseDto.CanEdit;
        CanDelete = responseDto.CanDelete;
    }
    #endregion
    
    #region Properties
    public bool CanEdit { get; }
    public bool CanDelete { get; }
    #endregion
}