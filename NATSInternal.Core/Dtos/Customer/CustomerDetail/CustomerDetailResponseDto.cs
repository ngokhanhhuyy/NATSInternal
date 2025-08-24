namespace NATSInternal.Core.Dtos;

public record CustomerDetailResponseDto : IUpsertableDetailResponseDto<CustomerExistingAuthorizationResponseDto>
{
    #region Properties
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }
    public string FullName { get; set; }
    public string? NickName { get; set; }
    public Gender Gender { get; set; }
    public DateOnly? Birthday { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ZaloNumber { get; set; }
    public string? FacebookUrl { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Note { get; set; }
    public UserBasicResponseDto CreatedUser { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public DateTime? LastUpdatedDateTime { get; set; }
    public CustomerBasicResponseDto? Introducer { get; set; }
    public long? RemainingDebtAmount { get; set; }
    public List<DebtBasicResponseDto> Debts { get; set; }
    public CustomerExistingAuthorizationResponseDto Authorization { get; set; }
    #endregion

    #region Constructors
    internal CustomerDetailResponseDto(Customer customer, CustomerExistingAuthorizationResponseDto authorization)
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
        LastUpdatedDateTime = customer.LastUpdatedDateTime;
        RemainingDebtAmount = customer.RemainingDebtAmount;
        Authorization = authorization;
        Introducer = customer.Introducer is not null ? new CustomerBasicResponseDto(customer.Introducer) : null;
        Debts = customer.Debts.Select(d => new DebtBasicResponseDto(d)).ToList();
    }
    #endregion
}