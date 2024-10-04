namespace NATSInternal.Services.Interfaces.Dtos;

public interface IFinancialEngageableBasicResponseDto<TAuthorization>
    : IUpsertableBasicResponseDto<TAuthorization>
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto
{
    long Amount { get; internal set; }
    bool IsLocked { get; internal set; }
}