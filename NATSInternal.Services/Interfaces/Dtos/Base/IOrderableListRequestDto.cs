namespace NATSInternal.Services.Interfaces.Dtos;

public interface IOrderableListRequestDto : IListRequestDto
{
    bool OrderByAscending { get; set; }
    string OrderByField { get; set; }
}