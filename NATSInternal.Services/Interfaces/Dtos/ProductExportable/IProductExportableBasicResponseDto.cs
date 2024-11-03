namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IProductExportableBasicResponseDto<TExistingAuthorization>
    : ICustomerEngageableBasicResponseDto<TExistingAuthorization>
    where TExistingAuthorization : IFinancialEngageableExistingAuthorizationResponseDto;