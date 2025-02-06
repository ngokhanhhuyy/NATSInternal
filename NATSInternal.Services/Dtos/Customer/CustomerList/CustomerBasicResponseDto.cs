namespace NATSInternal.Services.Dtos;

public class CustomerBasicResponseDto
    : IUpsertableBasicResponseDto<CustomerExistingAuthorizationResponseDto>
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string NickName { get; set; }
    public Gender Gender { get; set; }
    public DateOnly? Birthday { get; set; }
    public string PhoneNumber { get; set; }
    public long? DebtAmount { get; set; }
    public CustomerExistingAuthorizationResponseDto Authorization { get; set; }

    internal CustomerBasicResponseDto(Customer customer)
    {
        MapFromEntity(customer);
    }

    internal CustomerBasicResponseDto(
            Customer customer,
            CustomerExistingAuthorizationResponseDto authorization)
    {
        MapFromEntity(customer);
        Authorization = authorization;
    }

    private void MapFromEntity(Customer customer)
    {
        Id = customer.Id;
        FullName = customer.FullName;
        NickName = customer.NickName;
        Gender = customer.Gender;
        Birthday = customer.Birthday;
        PhoneNumber = customer.PhoneNumber;
        DebtAmount = customer.CachedDebtAmount;
    }
}