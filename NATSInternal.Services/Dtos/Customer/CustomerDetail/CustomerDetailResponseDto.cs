namespace NATSInternal.Services.Dtos;

using DebtIncurrenceAuthorizationGetter = Func<DebtIncurrence, DebtIncurrenceExistingAuthorizationResponseDto>;
using DebtPaymentAuthorizationGetter = Func<DebtPayment, DebtPaymentExistingAuthorizationResponseDto>;

public record CustomerDetailResponseDto
    : IUpsertableDetailResponseDto<CustomerExistingAuthorizationResponseDto>
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string NickName { get; set; }
    public Gender Gender { get; set; }
    public DateOnly? Birthday { get; set; }
    public string PhoneNumber { get; set; }
    public string ZaloNumber { get; set; }
    public string FacebookUrl { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public string Note { get; set; }
    public UserBasicResponseDto CreatedUser { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime? UpdatedDateTime { get; set; }
    public CustomerBasicResponseDto Introducer { get; set; }
    public long? DebtAmount { get; set; }
    public List<CustomerDebtOperationResponseDto> DebtOperations { get; set; }
    public CustomerExistingAuthorizationResponseDto Authorization { get; set; }

    internal CustomerDetailResponseDto(
            Customer customer,
            CustomerExistingAuthorizationResponseDto authorization,
            DebtIncurrenceAuthorizationGetter getDebtIncurrenceAuthorization,
            DebtPaymentAuthorizationGetter getDebtPaymentAuthorization)
    {
        Id = customer.Id;
        FirstName = customer.FirstName;
        MiddleName = customer.MiddleName;
        LastName = customer.LastName;
        FullName = customer.FullName;
        NickName = customer.NickName;
        Gender = customer.Gender;
        Birthday = customer.Birthday;
        PhoneNumber = customer.PhoneNumber;
        ZaloNumber = customer.ZaloNumber;
        FacebookUrl = customer.FacebookUrl;
        Email = customer.Email;
        Address = customer.Address;
        Note = customer.Note;
        CreatedUser = new UserBasicResponseDto(customer.CreatedUser);
        CreatedDateTime = customer.CreatedDateTime;
        UpdatedDateTime = customer.UpdatedDateTime;
        DebtAmount = customer.DebtAmount;
        Authorization = authorization;

        if (customer.Introducer != null)
        {
            Introducer = new CustomerBasicResponseDto(customer.Introducer);
        }
        
        if (customer.DebtIncurrences != null)
        {
            DebtOperations = new List<CustomerDebtOperationResponseDto>();
            foreach (DebtIncurrence debtIncurrence in customer.DebtIncurrences)
            {
                CustomerDebtOperationResponseDto operationResponseDto;
                operationResponseDto = new CustomerDebtOperationResponseDto(
                    debtIncurrence,
                    getDebtIncurrenceAuthorization(debtIncurrence));
                DebtOperations.Add(operationResponseDto);
            }
        }
        
        if (customer.DebtPayments != null)
        {
            DebtOperations ??= new List<CustomerDebtOperationResponseDto>();
            foreach (DebtPayment debtPayment in customer.DebtPayments)
            {
                CustomerDebtOperationResponseDto operationResponseDto;
                operationResponseDto = new CustomerDebtOperationResponseDto(
                    debtPayment,
                    getDebtPaymentAuthorization(debtPayment));
                DebtOperations.Add(operationResponseDto);
            }
        }

        DebtOperations = DebtOperations?
            .OrderByDescending(dp => dp.OperatedDateTime)
            .ToList();
    }
}