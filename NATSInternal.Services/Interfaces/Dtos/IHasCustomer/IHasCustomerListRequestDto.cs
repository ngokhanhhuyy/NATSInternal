namespace NATSInternal.Services.Interfaces.Dtos;

public interface IHasCustomerListRequestDto : ISortableListRequestDto
{
    int? CustomerId { get; set; }
}