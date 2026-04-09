using Microsoft.AspNetCore.Mvc;
using NATSInternal.Application.UseCases.Customers;
using NATSInternal.Application.UseCases.Shared;

namespace NATSInternal.Web.Models;

public class CustomerBasicModel
{
    #region Constructors
    public CustomerBasicModel(CustomerBasicResponseDto responseDto)
    {
        Id = responseDto.Id;
        FullName = responseDto.FullName;
        NickName = responseDto.NickName;
        IsDeleted = responseDto.IsDeleted;
    }
    
    public CustomerBasicModel(CustomerGetDetailResponseDto responseDto)
    {
        Id = responseDto.Id;
        FullName = responseDto.FullName;
        NickName = responseDto.NickName;
    }
    #endregion
    
    #region Properties
    public Guid Id { get; }
    public string FullName { get; }
    public string? NickName { get; }
    public bool IsDeleted { get; }
    #endregion
    
    #region ComputedProperties
    public string? AvatarUrl
    {
        get
        {
            if (string.IsNullOrWhiteSpace(FullName))
            {
                return null;
            }
            
            return $"https://ui-avatars.com/api/?name={FullName.Replace(" ", "+")}&background=random";
        }
    }
    #endregion
    
    #region Methods
    public string? GetDetailRoutePath(IUrlHelper urlHelper)
    {
        if (IsDeleted)
        {
            return null;
        }
        
        return urlHelper.Action("Detail", "Customer", new { id = Id });
    }
    #endregion
}