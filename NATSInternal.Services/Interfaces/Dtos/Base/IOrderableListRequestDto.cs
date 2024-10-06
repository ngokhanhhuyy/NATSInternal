namespace NATSInternal.Services.Interfaces.Dtos;

internal interface IOrderableListRequestDto : IListRequestDto
{
    bool OrderByAscending { get; set; }
    string OrderByField { get; set; }
}