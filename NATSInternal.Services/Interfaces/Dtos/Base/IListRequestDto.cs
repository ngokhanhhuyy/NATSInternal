namespace NATSInternal.Services.Interfaces.Dtos;

public interface IListRequestDto<TRequestDto> : IRequestDto<TRequestDto>
{
    bool OrderByAscending { get; set; }
    string OrderByField { get; set; }
    int Page { get; set; }
    int ResultsPerPage { get; set; }
}