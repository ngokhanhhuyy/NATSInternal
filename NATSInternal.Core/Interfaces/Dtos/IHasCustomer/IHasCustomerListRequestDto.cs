namespace NATSInternal.Core.Interfaces.Dtos;

public interface IHasCustomerListRequestDto : ISortableListRequestDto
{
    int? CustomerId { get; set; }
}