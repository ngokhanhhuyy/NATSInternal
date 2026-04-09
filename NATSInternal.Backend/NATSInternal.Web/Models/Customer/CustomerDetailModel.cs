using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using NATSInternal.Application.Localization;
using NATSInternal.Application.UseCases.Customers;
using NATSInternal.Domain.Features.Customers;

namespace NATSInternal.Web.Models;

public class CustomerDetailModel
{
    #region Constructors
    public CustomerDetailModel(CustomerGetDetailResponseDto responseDto)
    {
        Id = responseDto.Id;
        FirstName = responseDto.FirstName;
        MiddleName = responseDto.MiddleName;
        LastName = responseDto.LastName;
        FullName = responseDto.FullName;
        NickName = responseDto.NickName;
        Gender = responseDto.Gender;
        Birthday = responseDto.Birthday;
        PhoneNumber = responseDto.PhoneNumber;
        ZaloNumber = responseDto.ZaloNumber;
        FacebookUrl = responseDto.FacebookUrl;
        Email = responseDto.Email;
        Address = responseDto.Address;
        Note = responseDto.Note;
        CreatedUser = new(responseDto.CreatedUser);
        CreatedDateTime = responseDto.CreatedDateTime;
        LastUpdatedUser = responseDto.LastUpdatedUser is not null ? new(responseDto.LastUpdatedUser) : LastUpdatedUser;
        LastUpdatedDateTime = responseDto.LastUpdatedDateTime;
        DebtRemainingAmount = responseDto.DebtRemainingAmount;
        Introducer = responseDto.Introducer is not null ? new(responseDto.Introducer) : Introducer;
        Authorization = new(responseDto.Authorization);
    }
    #endregion
    
    #region Properties
    [DisplayName(DisplayNames.Id)]
    public Guid Id { get; }
    
    [DisplayName(DisplayNames.FirstName)]
    public string FirstName { get; }
    
    [DisplayName(DisplayNames.MiddleName)]
    public string? MiddleName { get; }
    
    [DisplayName(DisplayNames.LastName)]
    public string LastName { get; }
    
    [DisplayName(DisplayNames.FullName)]
    public string FullName { get; }
    
    [DisplayName(DisplayNames.NickName)]
    public string? NickName { get; }
    
    [DisplayName(DisplayNames.Gender)]
    public Gender Gender { get; }
    
    [DisplayName(DisplayNames.Birthday)]
    public DateOnly? Birthday { get; }
    
    [DisplayName(DisplayNames.PhoneNumber)]
    public string? PhoneNumber { get; }
    
    [DisplayName(DisplayNames.ZaloNumber)]
    public string? ZaloNumber { get; }
    
    [DisplayName(DisplayNames.FacebookUrl)]
    public string? FacebookUrl { get; }
    
    [DisplayName(DisplayNames.Email)]
    public string? Email { get; }
    
    [DisplayName(DisplayNames.Address)]
    public string? Address { get; }
    
    [DisplayName(DisplayNames.Note)]
    public string? Note { get; }
    
    [DisplayName(DisplayNames.CreatedUser)]
    public UserBasicModel CreatedUser { get; }
    
    [DisplayName(DisplayNames.CreatedDateTime)]
    public DateTime CreatedDateTime { get; }
    
    [DisplayName(DisplayNames.LastUpdatedUser)]
    public UserBasicModel? LastUpdatedUser { get; }
    
    [DisplayName(DisplayNames.LastUpdatedDateTime)]
    public DateTime? LastUpdatedDateTime { get; }
    
    [DisplayName(DisplayNames.DebtRemainingAmount)]
    public long DebtRemainingAmount { get; }
    
    [DisplayName(DisplayNames.Introducer)]
    public CustomerBasicModel? Introducer { get; }
    
    public CustomerExistingAuthorizationModel Authorization { get; }
    #endregion
    
    #region ComputedProperties
    public string AvatarUrl => $"https://ui-avatars.com/api/?name={FullName.Replace(" ", "+")}&background=random";
    #endregion
    
    #region Methods
    public string GetUpdateRoutePath(IUrlHelper urlHelper)
    {
        return urlHelper.Action("Update", "Customer", new { id = Id }) ?? "#";
    }
    #endregion
}