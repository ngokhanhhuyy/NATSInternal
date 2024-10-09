namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IFinancialEngageableBasicResponseDto<TAuthorization>
    : IUpsertableBasicResponseDto<TAuthorization>
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto
{
    long Amount { get; set; }
    bool IsLocked { get; set; }
}