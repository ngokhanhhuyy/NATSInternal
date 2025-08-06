namespace NATSInternal.Core.Interfaces.Dtos;

public interface ISortableListRequestDto : IListRequestDto
{
    bool? SortingByAscending { get; set; }
    string SortingByField { get; set; }
}