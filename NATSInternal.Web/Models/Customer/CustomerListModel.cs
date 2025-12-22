using NATSInternal.Application.Authorization;
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
    public string FullName { get; }
    public string? NickName { get; }
    public Gender Gender { get; }
    public DateOnly? Birthday { get; }
    public string? PhoneNumber { get; }
    public long DebtRemainingAmount { get; }
    public CustomerExistingAuthorizationResponseDto Authorization { get; }
    #endregion
}