namespace NATSInternal.Services.Dtos;

public class CustomerBasicResponseDto
    : IUpsertableBasicResponseDto<CustomerAuthorizationResponseDto>
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string NickName { get; set; }
    public Gender Gender { get; set; }
    public DateOnly? Birthday { get; set; }
    public string PhoneNumber { get; set; }
    public long? DebtAmount { get; set; }
    public CustomerAuthorizationResponseDto Authorization { get; set; }

    internal CustomerBasicResponseDto(Customer customer)
    {
        MapFromEntity(customer);
    }

    internal CustomerBasicResponseDto(
            Customer customer,
            CustomerAuthorizationResponseDto authorization)
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

        if (customer.DebtIncurrences != null && customer.DebtPayments != null)
        {
            DebtAmount = customer.DebtAmount;
        }
    }
}