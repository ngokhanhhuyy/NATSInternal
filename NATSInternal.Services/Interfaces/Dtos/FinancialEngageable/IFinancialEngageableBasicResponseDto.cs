namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IFinancialEngageableBasicResponseDto<TExistingAuthorization>
    : IUpsertableBasicResponseDto<TExistingAuthorization>
    where TExistingAuthorization : IFinancialEngageableExistingAuthorizationResponseDto
{
    DateTime StatsDateTime { get; }
    long AmountAfterVat { get; }
    bool IsLocked { get; }
}