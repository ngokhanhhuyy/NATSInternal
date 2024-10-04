namespace NATSInternal.Services.Interfaces.Dtos;

public interface ICustomerBasicResponseDto<TAuthorization>
    : IUpsertableBasicResponseDto<TAuthorization>
    where TAuthorization : IUpsertableAuthorizationResponseDto
{
    string FullName { get; internal set; }
    string NickName { get; internal set; }
    Gender Gender { get; internal set; }
    DateOnly? Birthday { get; internal set; }
    string PhoneNumber { get; internal set; }
    long? DebtAmount { get; internal set; }
}