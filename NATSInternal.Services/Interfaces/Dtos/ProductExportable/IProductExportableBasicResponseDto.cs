namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IProductExportableBasicResponseDto<TAuthorization>
    : ICustomerEngageableBasicResponseDto<TAuthorization>
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto;