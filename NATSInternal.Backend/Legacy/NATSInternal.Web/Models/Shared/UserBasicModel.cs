using Microsoft.AspNetCore.Mvc;
using NATSInternal.Application.UseCases.Shared;

namespace NATSInternal.Web.Models;

public class UserBasicModel
{
    #region Constructors
    public UserBasicModel(UserBasicResponseDto responseDto)
    {
        Id = responseDto.Id;
        UserName = responseDto.UserName;
        IsDeleted = responseDto.IsDeleted;
    }
    #endregion
    
    #region Properties
    public Guid Id { get; }
    public string UserName { get; }
    public bool IsDeleted { get; }
    #endregion
    
    #region Methods
    public string GetDetailRoute(IUrlHelper urlHelper)
    {
        return urlHelper.Action("Detail", "User", new { id = Id })!;
    }
    #endregion
}