using NATSInternal.Application.Authorization;
using NATSInternal.Domain.Features.Customers;

namespace NATSInternal.Application.UseCases.Customers;

public class CustomerGetListResponseDto : IPageableListResponseDto<CustomerGetListCustomerResponseDto>
{
    #region Constructors
    public CustomerGetListResponseDto(ICollection<CustomerGetListCustomerResponseDto> responseDtos, int pageCount)
    {
        Items = responseDtos;
        PageCount = pageCount;
    }
    #endregion

    #region Properties
    public ICollection<CustomerGetListCustomerResponseDto> Items { get; }
    public int PageCount { get; }
    #endregion
}

public class CustomerGetListCustomerResponseDto
{
    #region Constructors
    internal CustomerGetListCustomerResponseDto(
        Customer customer,
        CustomerExistingAuthorizationResponseDto authorization)
    {
        Id = customer.Id;
        FullName = customer.FullName;
        NickName = customer.NickName;
        Gender = customer.Gender;
        Birthday = customer.Birthday;
        PhoneNumber = customer.PhoneNumber;
        Authorization = authorization;
    }
    #endregion

    #region Properties
    public Guid Id { get; } = Guid.Empty;
    public string FullName { get; } = string.Empty;
    public string? NickName { get; }
    public Gender? Gender { get; } = Domain.Features.Customers.Gender.Male;
    public DateOnly? Birthday { get; }
    public string? PhoneNumber { get; }
    public long DebtRemainingAmount { get; }
    public CustomerExistingAuthorizationResponseDto Authorization { get; }
    #endregion
}