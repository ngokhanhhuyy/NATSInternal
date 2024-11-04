namespace NATSInternal.Services.Interfaces.Dtos;

public interface ICustomerEngageableListRequestDto : ISortableListRequestDto
{
    int? CustomerId { get; set; }
}