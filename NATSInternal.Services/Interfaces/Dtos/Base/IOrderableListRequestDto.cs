namespace NATSInternal.Services.Interfaces.Dtos;

public interface IOrderableListRequestDto : IListRequestDto
{
    bool? SortingByAscending { get; set; }
    string SortingByField { get; set; }
}