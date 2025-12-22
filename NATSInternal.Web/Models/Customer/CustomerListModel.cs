using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NATSInternal.Application.Authorization;
using NATSInternal.Application.Localization;
using NATSInternal.Application.UseCases.Customers;
using NATSInternal.Domain.Features.Customers;

namespace NATSInternal.Web.Models;

public class CustomerListModel : AbstractListModel<
    CustomerListCustomerModel,
    CustomerGetListRequestDto,
    CustomerGetListResponseDto,
    CustomerGetListCustomerResponseDto,
    CustomerGetListRequestDto.FieldToSort>
{
    #region ProtectedMethods
    protected override void MapItemsFromResponseDtos(IEnumerable<CustomerGetListCustomerResponseDto> responseDtos)
    {
        Items = responseDtos
            .Select(dto => new CustomerListCustomerModel(dto))
            .ToList()
            .AsReadOnly();
    }
    #endregion
}

public class CustomerListCustomerModel
{
    #region Constructors
    public CustomerListCustomerModel(CustomerGetListCustomerResponseDto responseDto)
    {
        Id = responseDto.Id;
        FullName = responseDto.FullName;
        NickName = responseDto.NickName;
        Gender = responseDto.Gender;
        Birthday = responseDto.Birthday;
        PhoneNumber = responseDto.PhoneNumber;
        DebtRemainingAmount = responseDto.DebtRemainingAmount;
        Authorization = responseDto.Authorization;
    }
    #endregion

    #region Properties
    public Guid Id { get; }
    
    [DisplayName(DisplayNames.FullName)]
    public string FullName { get; }
    
    [DisplayName(DisplayNames.NickName)]
    public string? NickName { get; }
    
    [DisplayName(DisplayNames.Gender)]
    public Gender Gender { get; }
    
    [DisplayName(DisplayNames.Birthday)]
    [DisplayFormat(DataFormatString = "Ngày {0:dd} tháng {0:MM}, năm {0:yyyy}")]
    public DateOnly? Birthday { get; }
    
    [DisplayName(DisplayNames.PhoneNumber)]
    [DisplayFormat(DataFormatString = "{0:(####) ###-####}")]
    public string? PhoneNumber { get; }
    
    [DisplayName(DisplayNames.DebtRemainingAmount)]
    [DisplayFormat(DataFormatString = "{0:N0} ₫")]
    public long DebtRemainingAmount { get; }
    
    public CustomerExistingAuthorizationResponseDto Authorization { get; }
    #endregion
}