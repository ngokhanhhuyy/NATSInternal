namespace NATSInternal.Core.Dtos;

public class CustomerBasicResponseDto : IUpsertableBasicResponseDto<CustomerExistingAuthorizationResponseDto>
{
    #region Properties
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string? NickName { get; set; }
    public Gender Gender { get; set; }
    public DateOnly? Birthday { get; set; }
    public string? PhoneNumber { get; set; }
    public long? RemainingDebtAmount { get; set; }
    public AnnouncementExistingAuthorizationResponseDto? Authorization { get; set; }
    #endregion

    #region Constructors
    internal CustomerBasicResponseDto(Customer customer)
    {
        Id = customer.Id;
        FullName = customer.FullName;
        NickName = customer.NickName;
        Gender = customer.Gender;
        Birthday = customer.Birthday;
        PhoneNumber = customer.PhoneNumber;
        RemainingDebtAmount = customer.CachedDebtAmount;
    }

    internal CustomerBasicResponseDto(
            Customer customer,
            CustomerExistingAuthorizationResponseDto authorization) : this(customer)
    {
        Authorization = authorization;
    }
    #endregion
}