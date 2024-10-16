namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IFinancialEngageableBasicResponseDto<TAuthorization>
    : IUpsertableBasicResponseDto<TAuthorization>
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto
{
    long AmountBeforeVat { get; }
    bool IsLocked { get; }
}