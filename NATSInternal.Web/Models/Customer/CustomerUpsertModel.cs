using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NATSInternal.Application.Localization;
using NATSInternal.Application.UseCases.Customers;
using NATSInternal.Application.UseCases.Shared;
using NATSInternal.Domain.Features.Customers;

namespace NATSInternal.Web.Models;

public class CustomerUpsertModel
{
    #region Constructors
    public CustomerUpsertModel() { }

    public CustomerUpsertModel(CustomerGetDetailResponseDto responseDto)
    {
        FirstName = responseDto.FirstName;
        MiddleName = responseDto.MiddleName;
        LastName = responseDto.LastName;
        NickName = responseDto.NickName;
        Gender = responseDto.Gender;
        Birthday = responseDto.Birthday;
        PhoneNumber = responseDto.PhoneNumber;
        ZaloNumber = responseDto.ZaloNumber;
        FacebookUrl = responseDto.FacebookUrl;
        Email = responseDto.Email;
        Address = responseDto.Address;
        Note = responseDto.Note;

        if (responseDto.Introducer is not null)
        {
            PickedIntroducerId = responseDto.Introducer.Id;
            PickedIntroducer = new(responseDto.Introducer);
        }
    }
    #endregion

    #region Properties
    [BindNever]
    [DisplayName(DisplayNames.Id)]
    public Guid Id { get; set; } = Guid.Empty;
    
    [BindRequired]
    [FromForm]
    [DisplayName(DisplayNames.FirstName)]
    public string FirstName { get; init; } = string.Empty;

    [BindRequired]
    [FromForm]
    [DisplayName(DisplayNames.MiddleName)]
    public string? MiddleName { get; init; }

    [BindRequired]
    [FromForm]
    [DisplayName(DisplayNames.LastName)]
    public string LastName { get; init; } = string.Empty;

    [BindRequired]
    [FromForm]
    [DisplayName(DisplayNames.NickName)]
    public string? NickName { get; init; }

    [BindRequired]
    [FromForm]
    [DisplayName(DisplayNames.Birthday)]
    public Gender Gender { get; init; }
    public DateOnly? Birthday { get; init; }

    [BindRequired]
    [FromForm]
    [DisplayName(DisplayNames.PhoneNumber)]
    public string? PhoneNumber { get; init; }

    [BindRequired]
    [FromForm]
    [DisplayName(DisplayNames.ZaloNumber)]
    public string? ZaloNumber { get; init; }

    [BindRequired]
    [FromForm]
    [DisplayName(DisplayNames.FacebookUrl)]
    public string? FacebookUrl { get; init; }

    [BindRequired]
    [FromForm]
    [DisplayName(DisplayNames.Email)]
    public string? Email { get; init; }

    [BindRequired]
    [FromForm]
    [DisplayName(DisplayNames.Address)]
    public string? Address { get; init; }

    [BindRequired]
    [FromForm]
    [DisplayName(DisplayNames.Note)]
    public string? Note { get; init; }

    [BindRequired]
    [DisplayName(DisplayNames.Introducer)]
    public Guid? PickedIntroducerId { get; set; }
    
    [BindNever]
    [DisplayName(DisplayNames.Introducer)]
    public CustomerBasicModel? PickedIntroducer { get; set; }

    [FromQuery]
    public CustomerListModel CustomerList { get; set; } = new();

    [BindRequired]
    public SubmitAction Action { get; set; } = SubmitAction.RedirectionToIntroducerPicker;
    #endregion

    #region Methods
    public CustomerCreateRequestDto ToCreateRequestDto()
    {
        CustomerCreateRequestDto requestDto = new();
        MapToRequestDto(requestDto);
        return requestDto;
    }

    public CustomerUpdateRequestDto ToUpdateRequestDto()
    {
        CustomerUpdateRequestDto requestDto = new();
        MapToRequestDto(requestDto);
        return requestDto;
    }

    public CustomerValidateRequestDto ToValidateRequestDto()
    {
        CustomerValidateRequestDto requestDto = new()
        {
            Data = new()
        };
        
        MapToRequestDto(requestDto.Data);
        return requestDto;
    }

    public void MapFromPickedIntroducerResponseDto(CustomerBasicResponseDto? responseDto)
    {
        if (responseDto is not null)
        {
            PickedIntroducer = new(responseDto);
        }
    }

    public void MapFromPickedIntroducerResponseDto(CustomerGetDetailResponseDto responseDto)
    {
        PickedIntroducer = new(responseDto);
    }
    #endregion

    #region PrivateMethods
    protected void MapToRequestDto(CustomerUpsertRequestDto requestDto)
    {
        requestDto.FirstName = FirstName;
        requestDto.MiddleName = MiddleName;
        requestDto.LastName = LastName;
        requestDto.NickName = NickName;
        requestDto.Gender = Gender;
        requestDto.Birthday = Birthday;
        requestDto.PhoneNumber = PhoneNumber;
        requestDto.ZaloNumber = ZaloNumber;
        requestDto.FacebookUrl = FacebookUrl;
        requestDto.Email = Email;
        requestDto.Address = Address;
        requestDto.Note = Note;
        requestDto.IntroducerId = PickedIntroducerId;
    }
    #endregion

    #region Enums
    public enum SubmitAction
    {
        RedirectionToIntroducerPicker,
        IntroducerPickingOrUnpicking,
        Confirmation,
        FinalSubmit
    }
    #endregion
}