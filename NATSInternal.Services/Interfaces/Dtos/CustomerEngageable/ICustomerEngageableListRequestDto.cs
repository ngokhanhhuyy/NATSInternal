namespace NATSInternal.Services.Interfaces.Dtos;

internal interface ICustomerEngageableListRequestDto : IOrderableListRequestDto
{
    int? CustomerId { get; internal set; }
}