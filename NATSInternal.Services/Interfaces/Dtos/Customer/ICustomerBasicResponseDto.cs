namespace NATSInternal.Services.Interfaces.Dtos;

public interface ICustomerBasicResponseDto<TAuthorization>
    : IUpsertableBasicResponseDto<TAuthorization>
    where TAuthorization : IUpsertableAuthorizationResponseDto
{
    string FullName { get; set; }
    string NickName { get; set; }
    Gender Gender { get; set; }
    DateOnly? Birthday { get; set; }
    string PhoneNumber { get; set; }
    long? DebtAmount { get; set; }
}