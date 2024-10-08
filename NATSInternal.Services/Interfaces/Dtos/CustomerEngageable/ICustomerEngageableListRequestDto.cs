namespace NATSInternal.Services.Interfaces.Dtos;

public interface ICustomerEngageableListRequestDto : IOrderableListRequestDto
{
    int? CustomerId { get; set; }
}