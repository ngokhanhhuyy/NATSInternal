namespace NATSInternal.Services.Interfaces.Dtos;

/// <summary>
/// A DTO containing the basic information of an entity which represents an transaction that
/// can be paid and can generate income.
/// </summary>
/// <typeparam name="TAuthorization">
/// The type of the authorization DTO, containing the information of the permissions to
/// interact with the entity.
/// </typeparam>
public interface IPayableBasicResponseDto<TCustomer, TAuthorization>
    : ICustomerFinancialEngageableBasicResponseDto<TCustomer, TAuthorization>
    where TAuthorization : IFinancialEngageableAuthorizationResponseDto
{
    /// <summary>
    /// Represents the date and time when the transaction is paid.
    /// </summary>
    DateTime PaidDateTime { get; internal set; }
}